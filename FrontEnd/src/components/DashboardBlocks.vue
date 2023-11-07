<template>
  <div class="container">
    <div class="div1">
      <h1 class="div1Text">Welcome to HR Dashboard!👋</h1>
    </div>
    <div class="div2">
      <h1 class="div2Text">⚡Employees work status statistics:</h1>
      <div class="chart-container">
        <Pie :data="piechartData" :options="pieoptions" />
      </div>
    </div>
    <div class="div2">
      <div class="div2Text">⚡Schedule</div>
      <div class="schedule">
        <div class="today-date">{{ today.toLocaleDateString() }}</div>
        <div class="shifts">
          <div v-for="shift in todayShifts" :key="shift.id">
            Employee Name: {{ shift.startTime }} - {{ shift.endTime }} ({{
              shift.title
            }})
          </div>
          <div v-if="todayShifts.length === 0">No shifts for today.</div>
        </div>
      </div>
    </div>
    <div class="div2">
      <h1 class="div2Text">⚡Employees salary statistics:</h1>
      <div class="chart-container">
        <Bar :data="barchartData" :options="baroptions" />
      </div>
    </div>
    <div class="div2">
      <div class="div2Text">⚡Notifications</div>
      <div v-if="lastTwoNotifications.length === 0"> Notifications not found </div>
      <div class="notification-block" v-for="notification in lastTwoNotifications" :key="notification.notificationId">
        <div>{{new Date(notification.date).toLocaleDateString([], { hour: '2-digit', minute: '2-digit' }) }}  {{ notification.type }}</div>
      </div>
    </div>     
    </div>  
</template>

<script setup lang="ts">
import { useEmployeesStore } from '@/stores/employeesStore';
import { useShiftsStore } from '@/stores/shiftsStore';
import { useNotificationsStore } from '@/stores/notificationsStore';
import { onMounted, ref, watch, computed } from 'vue';
import { Pie } from 'vue-chartjs';
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
  Title,
} from 'chart.js';
import { Bar } from 'vue-chartjs';

ChartJS.register(ArcElement, Tooltip, Legend);
ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
);

defineProps<{ name: String }>();

const employeesStore = useEmployeesStore();
const employeesNameFilter = ref<string>('');
const today = new Date();
const shiftsStore = useShiftsStore();

const notificationsStore = useNotificationsStore();

const todayShifts = computed(() => {
  return shiftsStore.shifts.filter((shift) => {
    const shiftDate = new Date(shift.date);
    return shiftDate.toDateString() === today.toDateString();
  });
});

const lastTwoNotifications = computed(() => {
  return notificationsStore.notifications.slice(-3).reverse(); 
});

onMounted(() => {
  employeesStore.load();
  shiftsStore.load();
  notificationsStore.load();
});

const piechartData = computed(() => {
  return {
    labels: [
      'Active',
      'On Maternaty Leave',
      'On Vacation Leave',
      'On Leave Process',
    ],
    datasets: [
      {
        backgroundColor: ['#41B883', '#E46651', '#00D8FF', '#DD1B16'],
        data: [
          activeEmployeeCount.value, // Extract the actual value using .value
          onMaternatyLeaveEmployeeCount.value, // Extract the actual value
          onVacationLeaveEmployeeCount.value, // Extract the actual value
          onLeaveProcessEmployeeCount.value, // Extract the actual value
        ],
      },
    ],
  };
});

const barchartData = computed(() => {
  return {
    labels: [
      'Vendor',
      'Shift manager',
      'Warehouse worker',
      'Consultant',
      'Accountant',
    ],
    datasets: [
      {
        backgroundColor: [
          '#41B883',
          '#E46651',
          '#00D8FF',
          '#DD1B16',
          '#0000FF',
        ],
        data: [
          vendorEmployeesSalaryCount.value, // Extract the actual value using .value
          shiftManagerEmployeesSalaryCount.value, // Extract the actual value
          warehouseWorkerEmployeesSalaryCount.value, // Extract the actual value
          consultantEmployeesSalaryCount.value, // Extract the actual value
          accountantEmployeesSalaryCount.value,
        ],
      },
    ],
  };
});

const vendorEmployeesSalaryCount = computed(() => {
  return employeesStore.employees
    .filter((employee) => employee.position === 'Vendor')
    .reduce((totalSalary, employee) => totalSalary + employee.salary, 0);
});
const shiftManagerEmployeesSalaryCount = computed(() => {
  return employeesStore.employees
    .filter((employee) => employee.position === 'Shift Manager')
    .reduce((totalSalary, employee) => totalSalary + employee.salary, 0);
});
const warehouseWorkerEmployeesSalaryCount = computed(() => {
  return employeesStore.employees
    .filter((employee) => employee.position === 'Warehouse worker')
    .reduce((totalSalary, employee) => totalSalary + employee.salary, 0);
});
const consultantEmployeesSalaryCount = computed(() => {
  return employeesStore.employees
    .filter((employee) => employee.position === 'Consultant')
    .reduce((totalSalary, employee) => totalSalary + employee.salary, 0);
});
const accountantEmployeesSalaryCount = computed(() => {
  return employeesStore.employees
    .filter((employee) => employee.position === 'Accountant')
    .reduce((totalSalary, employee) => totalSalary + employee.salary, 0);
});

const activeEmployeeCount = computed(() => {
  return employeesStore.employees.filter(
    (employee) => employee.status === 'Active',
  ).length;
});
const onMaternatyLeaveEmployeeCount = computed(() => {
  return employeesStore.employees.filter(
    (employee) => employee.status === 'OnMaternityLeave',
  ).length;
});
const onVacationLeaveEmployeeCount = computed(() => {
  return employeesStore.employees.filter(
    (employee) => employee.status === 'OnVaction',
  ).length;
});

const onLeaveProcessEmployeeCount = computed(() => {
  return employeesStore.employees.filter(
    (employee) => employee.status === 'OnLeaveProcess',
  ).length;
});

const pieoptions = {
  responsive: true,
  maintainAspectRatio: false,
};
const baroptions = {
  responsive: true,
  plugins: {
    legend: {
      display: false,
    },
  },
};

watch(employeesNameFilter, (name) => {
  employeesStore.filterEmployeesByName(name);
});
</script>

<style scoped>
.container {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  grid-gap: 10px;
  height: 1000px;
}

.div1 {
  grid-column: 1 / 4;
  background-color: white;
  border: solid;
  border-radius: 25px;
  border-color: #ccccff;
  font-size: x-large;
  font-weight: bold;
  text-align: center;
}
.div6 {
  grid-column: 1 / 4;
  background-color: white;
  border: solid;
  border-radius: 25px;
  border-color: #ccccff;
  font-size: x-large;
  font-weight: bold;
}
.div1Text {
  align-items: center;
  margin-bottom: 10px;
  font-size: xx-large;
  text-align: center;
}
.div2 {
  background-color: white;
  border: solid;
  border-radius: 25px;
  border-color: #ccccff;
  font-size: x-large;
  font-weight: bold;
  background-color: white;
  text-align: center;
}
.div2Text {
  align-items: center;
  margin-bottom: 10px;
  font-size: large;
}
.div3,
.div4,
.div5 {
  background-color: #3399ff;
  height: 100px;
}
.today-date {
  background-color: rgba(199, 210, 254);
  text-align: center;
  font-size: 150%;
}
.shifts {
  font-weight: normal;
  margin-left: 20px;
}
.notification-block{
  background-color: rgba(199, 210, 254, 0.294);
  padding: 10px;
  border-radius: 8px;
  margin-top: 10px;
}
@media (max-width: 1900px) {
  .container {
    grid-template-columns: 1fr 1fr;
    grid-template-rows: repeat(5, 1fr);
  }

  .div1 {
    grid-column: 1 / 3;
  }
  .div6 {
    grid-column: 1 / 3;
  }
}
</style>
