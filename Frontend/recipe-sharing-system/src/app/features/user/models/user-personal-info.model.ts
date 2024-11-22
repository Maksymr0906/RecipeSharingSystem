export interface UserPersonalInfo {
    id: string;
    email: string;
    userName: string;
    firstName?: string;
    lastName?: string;
    dateOfBirth?: Date;
    postalCode?: string;
    authoredRecipeIds: string[];
    favoriteRecipeIds: string[];
    reviewIds: string[];
}