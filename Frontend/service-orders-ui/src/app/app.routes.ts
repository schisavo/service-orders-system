import { Routes } from '@angular/router';
import { DashboardLayout } from './layouts/dashboard/dashboard-layout';
import { ClientList } from './features/client/list/client-list';
import { ServiceOrderList } from './features/service-order/list/service-order-list';
import { Login } from './features/auth/login/login';

export const routes: Routes = [
    {
    path: '',
    component: DashboardLayout,
        children: [
            //{ path: 'technicians', component: TechnicianListComponent },
            { path: 'clients', component: ClientList },
            { path: 'service-orders', component: ServiceOrderList }
        ]
    },
    { path: 'login', component: Login }
];

