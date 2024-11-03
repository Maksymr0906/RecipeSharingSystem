import { IngredientQuantity } from "./ingredient-quantity.model";

export interface Recipe {
    id: string
    title: string;
    shortDescription: string;
    slug: string;
    featuredImageUrl: string;
    instructionId: string;
    rating: number;
    categoryIds: string[];
    ingredients: IngredientQuantity[];
}