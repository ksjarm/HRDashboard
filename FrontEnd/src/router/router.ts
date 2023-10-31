import { RouteRecordRaw, createRouter, createWebHistory } from 'vue-router';
import EmployeesVue from '@/views/Employees.vue';
import AddEmployeeVue from '@/views/AddEmployee.vue';
import UpdateEmployeeVue from '@/views/UpdateEmployee.vue';
import SheduleVue from '@/components/Shedule.vue';
import Dashboard from '@/views/Dashboard.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Dashboard',
    component: Dashboard,
    props: { title: 'Dashboard' },
  },
  {
    path: '/employees',
    name: 'Employees',
    component: EmployeesVue,
    props: { title: 'Employees' },
  },
  {
    path: '/newemployee',
    name: 'Add employee',
    component: AddEmployeeVue,
  },
  {
    path: '/update/:id',
    name: 'Update employee ',
    component: UpdateEmployeeVue,
  },
  {
    path: '/shedule',
    name: 'Shedule',
    component: SheduleVue,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
