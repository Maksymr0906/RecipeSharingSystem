export interface LoginResponse {
    userId: string;
    email: string;
    token: string;
    roles: string[];
}