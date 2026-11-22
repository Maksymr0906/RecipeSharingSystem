// Lab 5, step 11: integrity and authenticity verification for release artifacts
// (the published backend and the production Angular bundle). Theory used here:
//   - SHA-256 alone (a MDC) only detects accidental corruption: anyone can
//     recompute a new, matching hash for a modified file, so a hash shipped
//     next to the artifact proves nothing about its origin (see docs/security/
//     integrity_and_distribution.md, "Why not a plain checksum / why not HMAC").
//   - Signing the manifest with the developer's RSA private key (RSA-PSS,
//     SHA-256) and shipping only the PUBLIC key with the verifier gives the
//     asymmetric separation the lab asks for: the person who verifies a
//     release can never produce a new valid signature for a tampered one.
//
// Usage:
//   release-integrity sign   --dir <releaseDir> --key <private_key.pem> --out <manifestDir>
//   release-integrity verify --dir <releaseDir> --manifest <manifestDir> --pubkey <public_key.pem>
//
// "sign" is run once by the developer, on the machine that holds the private
// key, right before publishing a release. "verify" is run by anyone who
// receives the release (a deployment script, or a self-hoster who downloaded
// it from GitHub) and only ever needs the public key.

using System.Security.Cryptography;
using System.Text;

namespace RecipeSharingSystem.ReleaseIntegrity;

internal static class Program
{
    private const string ManifestFileName = "manifest.sha256";
    private const string SignatureFileName = "manifest.sha256.sig";
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    private static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return 2;
        }

        try
        {
            return args[0] switch
            {
                "sign" => RunSign(args[1..]),
                "verify" => RunVerify(args[1..]),
                _ => Unknown(args[0])
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: {ex.Message}");
            return 2;
        }
    }

    private static int Unknown(string command)
    {
        Console.Error.WriteLine($"error: unknown command '{command}'");
        PrintUsage();
        return 2;
    }

    private static void PrintUsage()
    {
        Console.Error.WriteLine("""
            usage:
              release-integrity sign   --dir <releaseDir> --key <private_key.pem> --out <manifestDir>
              release-integrity verify --dir <releaseDir> --manifest <manifestDir> --pubkey <public_key.pem>
            """);
    }

    // ------------------------------------------------------------------ sign

    private static int RunSign(string[] args)
    {
        var opts = ParseOptions(args, "dir", "key", "out");
        var releaseDir = RequireDirectory(opts["dir"]);
        var manifestDir = opts["out"];
        Directory.CreateDirectory(manifestDir);

        var files = EnumerateReleaseFiles(releaseDir);
        if (files.Count == 0)
        {
            throw new InvalidOperationException($"no files found under '{releaseDir}'");
        }

        var manifestText = BuildManifestText(releaseDir, files);
        var manifestPath = Path.Combine(manifestDir, ManifestFileName);
        File.WriteAllText(manifestPath, manifestText, new UTF8Encoding(false));

        using var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(opts["key"]));

        // RSA-PSS rather than the older PKCS#1 v1.5 padding, as recommended
        // in the lab's theoretical material for new signature schemes.
        var signature = rsa.SignData(
            Encoding.UTF8.GetBytes(manifestText),
            HashAlgorithm,
            RSASignaturePadding.Pss);

        File.WriteAllText(
            Path.Combine(manifestDir, SignatureFileName),
            Convert.ToBase64String(signature));

        Console.WriteLine($"signed {files.Count} file(s) from '{releaseDir}'");
        Console.WriteLine($"  manifest  -> {manifestPath}");
        Console.WriteLine($"  signature -> {Path.Combine(manifestDir, SignatureFileName)}");
        return 0;
    }

    // ---------------------------------------------------------------- verify

    private static int RunVerify(string[] args)
    {
        var opts = ParseOptions(args, "dir", "manifest", "pubkey");
        var releaseDir = RequireDirectory(opts["dir"]);
        var manifestDir = opts["manifest"];

        var manifestPath = Path.Combine(manifestDir, ManifestFileName);
        var signaturePath = Path.Combine(manifestDir, SignatureFileName);

        if (!File.Exists(manifestPath) || !File.Exists(signaturePath))
        {
            Console.Error.WriteLine("FAIL: manifest or signature file is missing");
            return 1;
        }

        var manifestText = File.ReadAllText(manifestPath, new UTF8Encoding(false));
        var signature = Convert.FromBase64String(File.ReadAllText(signaturePath).Trim());

        using var rsa = RSA.Create();
        rsa.ImportFromPem(File.ReadAllText(opts["pubkey"]));

        var signatureValid = rsa.VerifyData(
            Encoding.UTF8.GetBytes(manifestText),
            signature,
            HashAlgorithm,
            RSASignaturePadding.Pss);

        if (!signatureValid)
        {
            // The manifest itself was tampered with (or signed by someone
            // else's key) - there is no point checking individual file
            // hashes against a manifest we cannot trust.
            Console.Error.WriteLine("FAIL: signature does not match the manifest");
            return 1;
        }

        var declared = ParseManifest(manifestText);
        var actual = EnumerateReleaseFiles(releaseDir)
            .ToDictionary(f => f, f => ComputeHash(Path.Combine(releaseDir, f)));

        var ok = true;

        foreach (var (relativePath, expectedHash) in declared.OrderBy(kv => kv.Key, StringComparer.Ordinal))
        {
            if (!actual.TryGetValue(relativePath, out var actualHash))
            {
                Console.Error.WriteLine($"FAIL: missing file declared in the manifest: {relativePath}");
                ok = false;
                continue;
            }

            if (!string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine($"FAIL: hash mismatch for {relativePath}");
                ok = false;
            }
        }

        foreach (var relativePath in actual.Keys.Except(declared.Keys))
        {
            // An extra file is also a violation of the signed release: it was
            // either added after signing, or the attacker renamed a modified
            // file to avoid colliding with a declared path.
            Console.Error.WriteLine($"FAIL: file present but not declared in the manifest: {relativePath}");
            ok = false;
        }

        if (ok)
        {
            Console.WriteLine($"OK: signature valid, {declared.Count} file(s) match the signed manifest");
            return 0;
        }

        Console.Error.WriteLine("FAIL: release does not match its signed manifest");
        return 1;
    }

    // --------------------------------------------------------------- helpers

    private static List<string> EnumerateReleaseFiles(string releaseDir) =>
        Directory.EnumerateFiles(releaseDir, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(releaseDir, path).Replace('\\', '/'))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToList();

    private static string BuildManifestText(string releaseDir, IReadOnlyList<string> relativePaths)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# algorithm=SHA256 generated={DateTimeOffset.UtcNow:O}");

        foreach (var relativePath in relativePaths)
        {
            var hash = ComputeHash(Path.Combine(releaseDir, relativePath));
            sb.Append(hash).Append("  ").Append(relativePath).Append('\n');
        }

        return sb.ToString();
    }

    private static Dictionary<string, string> ParseManifest(string manifestText)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var line in manifestText.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.StartsWith('#'))
            {
                continue;
            }

            var separator = line.IndexOf("  ", StringComparison.Ordinal);
            if (separator < 0)
            {
                throw new InvalidOperationException($"malformed manifest line: '{line}'");
            }

            var hash = line[..separator];
            var path = line[(separator + 2)..].TrimEnd('\r');
            result[path] = hash;
        }

        return result;
    }

    private static string ComputeHash(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }

    private static string RequireDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"directory not found: '{path}'");
        }

        return path;
    }

    private static Dictionary<string, string> ParseOptions(string[] args, params string[] required)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        for (var i = 0; i < args.Length - 1; i++)
        {
            if (args[i].StartsWith("--", StringComparison.Ordinal))
            {
                result[args[i][2..]] = args[i + 1];
                i++;
            }
        }

        var missing = required.Where(name => !result.ContainsKey(name)).ToList();
        if (missing.Count > 0)
        {
            throw new ArgumentException($"missing required option(s): {string.Join(", ", missing.Select(m => "--" + m))}");
        }

        return result;
    }
}
