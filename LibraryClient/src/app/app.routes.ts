import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { authGuard } from './shared/auth.guard';
import { AdminOnlyComponent } from './authorizeDemo/admin-only/admin-only.component';
import { AdminOrLibrarianComponent } from './authorizeDemo/admin-or-librarian/admin-or-librarian.component';
import { MemberAccessComponent } from './authorizeDemo/member-access/member-access.component';
import { GuestAccessComponent } from './authorizeDemo/guest-access/guest-access.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { claimReq } from './shared/utils/claimReq-utils';
import { ForgotPasswordComponent } from './user/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './user/change-password/change-password.component';
import { BookListComponent } from './features/book/book-list/book-list.component';
import { BookFormComponent } from './features/book/book-form/book-form.component';
import { BookDetailComponent } from './user-hompage/book-detail/book-detail.component';
import { UserBookListComponent } from './user-hompage/user-book-list/user-book-list.component';
import { ProfileComponent } from './user/profile/profile.component';
import { BookBorrowedComponent } from './features/book/book-borrowed/book-borrowed.component';
import { BookBrowingComponent } from './features/book/book-browing/book-browing.component';
export const routes: Routes = [
    { path: '', redirectTo: 'signin', pathMatch: 'full' },
    {
        path: '', component: UserComponent,
        children: [
            { path: 'signup', component: RegistrationComponent },
            { path: 'signin', component: LoginComponent },
            { path: 'forgot-password', component: ForgotPasswordComponent },
            { path: 'change-password', component: ChangePasswordComponent },
        ]
    },
    {
        path: '', component: MainLayoutComponent,
        children: [
            {
                path: 'dashboard', component: DashboardComponent,
                canActivate: [authGuard]
            },
            {
                path: 'profile', component: ProfileComponent,
                canActivate: [authGuard]
            },
            {
                path: 'admin-only', component: AdminOnlyComponent,
                data: { claimReq: claimReq.adminOnly },
                canActivate: [authGuard]
            },
            {
                path: 'admin-or-librarian', component: AdminOrLibrarianComponent,
                data: { claimReq: claimReq.adminOrLibrarian },
                canActivate: [authGuard]
            },
            {
                path: 'member-access', component: MemberAccessComponent,
                data: { claimReq: claimReq.memberaccess },
                canActivate: [authGuard]
            },
            {
                path: 'guest-access', component: GuestAccessComponent
            },
            {
                path: 'forbidden', component: ForbiddenComponent,
                canActivate: [authGuard]
            },
            {
                path: 'book',
                component: BookListComponent,
                data: { claimReq: claimReq.adminOrLibrarian },
                canActivate: [authGuard]
            },
            {
                path: 'add-book',
                component: BookFormComponent,
                data: { claimReq: claimReq.adminOrLibrarian },
                canActivate: [authGuard]
            },
            {
                path: 'edit-book/:id',
                component: BookFormComponent,
                data: { claimReq: claimReq.adminOrLibrarian },
                canActivate: [authGuard]
            },
            {
                path: 'homepage',
                component: UserBookListComponent,
                data: { claimReq: claimReq.memberaccess },
                canActivate: [authGuard]
            },
            {
                path: 'book-detail/:id',
                component: BookDetailComponent,
                data: { claimReq: claimReq.memberaccess },
                canActivate: [authGuard]
            },
            {
                path: 'borrow-history', //Xem lịch sử mượn sách (tất cả sách đã mượn, đã trả, đang chờ yêu cầu mượn, quá hạn)
                component: BookBorrowedComponent,
                canActivate: [authGuard]
            },
            {
                path: 'borrowing-books', //lịch sử mượn sách (sách đang mượn)
                component: BookBrowingComponent,
                canActivate: [authGuard]
            }
        ]
    },
];
