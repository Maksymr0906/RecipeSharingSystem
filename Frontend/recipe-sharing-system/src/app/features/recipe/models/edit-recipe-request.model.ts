import { IngredientQuantity } from "./ingredient-quantity.model";

export interface EditRecipeRequest {
    title: string;
    shortDescription: string;
    instructionId: string;
    imageId: string;
    categoryIds: string[];
    ingredients: IngredientQuantity[];
}