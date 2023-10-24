import { RouteRecordRaw, createRouter, createWebHistory } from 'vue-router';
import ExercisesVue from '@/views/Exercises.vue';
import AddExerciseVue from '@/views/AddExercise.vue';
import UpdateExerciseVue from '@/views/UpdateExercise.vue';
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
    path: '/exercises',
    name: 'Harjutused',
    component: ExercisesVue,
    props: { title: 'Harjutused' },
  },
  {
    path: '/newexercise',
    name: 'Lisa harjutus',
    component: AddExerciseVue,
  },
  {
    path: '/update/:id',
    name: 'Uuenda harjutust',
    component: UpdateExerciseVue,
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
