export interface Review {
    id: string;
    rating: number;
    content?: string;
    dateCreated: Date;
    userName: string;
    userId: string;
    recipeId: string;
}