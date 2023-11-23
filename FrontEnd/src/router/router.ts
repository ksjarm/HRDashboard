import { RouteRecordRaw, createRouter, createWebHistory } from 'vue-router';
import EmployeesVue from '@/views/Employees.vue';
import AddEmployeeVue from '@/views/AddEmployee.vue';
import UpdateEmployeeVue from '@/views/UpdateEmployee.vue';
import SheduleVue from '@/components/Shedule.vue';
import Dashboard from '@/views/Dashboard.vue';
import NotificationHistoryVue from '@/views/NotificationHistory.vue';
import EmployeeFullInfo from '@/views/EmployeeFullInfo.vue';
import HRFullInfo from '@/views/HRFullInfo.vue';

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
  {
    path: '/notifications',
    name: 'Notifications',
    component: NotificationHistoryVue,
  },
  {
    path: '/fullinfo',
    name: 'Get full info',
    component: EmployeeFullInfo,
  },
  {
    path: '/hrfullinfo',
    name: 'Get HR full info',
    component: HRFullInfo,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
