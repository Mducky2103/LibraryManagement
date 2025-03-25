export interface Book {
    id: number;
    name: string;
    description: string;
    yearPublished: number;
    price: number;
    quantity: number;
    image: string;
    isAvailable: boolean;
    authorId: number;
    authorName: string;
    categoryId: number;
    categoryName: string;
}