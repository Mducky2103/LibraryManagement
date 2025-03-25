export const claimReq = {
    adminOnly: (c: any) => c.role == "Admin",
    adminOrLibrarian: (c: any) => c.role == "Admin" || c.role == "Librarian",
    memberaccess: (c: any) => c.role == "Member" || c.role == "Admin" || c.role == "Librarian"
}