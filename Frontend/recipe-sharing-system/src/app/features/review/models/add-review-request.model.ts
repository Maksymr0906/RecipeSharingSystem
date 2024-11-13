export interface AddReviewRequest {
    rating: number;
    content?: string;
    userId: string;
    recipeId: string;
}