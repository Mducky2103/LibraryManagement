export interface Penalty {
    id: string;
    username: string;
    borrowReceiptDetailId: number;
    bookName: string;
    amount: number;
    reason: string;
    issuedDate: Date;
    status: string;
}
