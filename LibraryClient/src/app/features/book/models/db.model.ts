export interface Borrow {
  id: number;
  bookName: string | null;
  borrowReceiptId: number;
  borrowReceipt: any | null;
  bookId: number;
  books: any | null;
  borrowedDate: Date | null;
  dueDate: Date | null;
  returnedDate: Date | null;
  status: number;
  yearPublished: number | null;
  notes: string | null;
  returnedBooks: any | null;
}
