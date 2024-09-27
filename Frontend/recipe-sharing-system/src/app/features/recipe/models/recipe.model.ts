import { IngredientQuantity } from "./ingredient-quantity.model";

export interface Recipe {
    id: string
    title: string;
    shortDescription: string;
    instructionId: string;
    imageId: string;
    ratingIds: string[];
    categoryIds: string[];
    ingredients: IngredientQuantity[];
}