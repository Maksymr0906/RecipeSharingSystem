import { IngredientQuantity } from "./ingredient-quantity.model";

export interface EditRecipeRequest {
    title: string;
    shortDescription: string;
    featuredImageUrl: string;
    instructionId: string;
    categoryIds: string[];
    ingredients: IngredientQuantity[];
}