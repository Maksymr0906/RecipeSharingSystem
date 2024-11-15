import { IngredientQuantity } from "./ingredient-quantity.model";

export interface AddRecipeRequest {
    title: string;
    shortDescription: string;
    slug: string;
    featuredImageUrl: string;
    instructionId: string;
    authorId: string;
    categoryIds: string[];
    ingredients: IngredientQuantity[];
}