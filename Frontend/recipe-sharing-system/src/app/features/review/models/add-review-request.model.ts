export interface AddReviewRequest {
    rating: number;
    content?: string;
    userId?: string; // ignored by the server: the author is taken from the login token
    recipeId: string;
}