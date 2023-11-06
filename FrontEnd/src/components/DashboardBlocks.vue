<template>
  <div class="container">
    <div class="div1">
      <div class="div1Text">Welcome to HR Dashboard!👋</div>
    </div>
    <div class="div2">
      <h1 class="div2Text">⚡Employees work status statistics:</h1>
      <div class="chart-container">
        <Pie :data="piechartData" :options="pieoptions" />
      </div>
    </div>
    <div class="div3">
      <div class="div3Text">Scheldule</div>
    </div>
    <div class="div2">
      <h1 class="div2Text">⚡Employees salary statistics:</h1>
      <div class="chart-container">
        <Bar :data="barchartData" :options="baroptions" />
      </div>
    </div>
    <div class="div5"><div class="div5Text">Notifications</div></div>
  </div>
</template>

<script setup lang="ts">
import { useEmployeesStore } from '@/stores/employeesStore';
import { onMounted, ref, watch, computed } from 'vue';
import {
  Chart as ChartJS,
  ArcElement,
  Tooltip,
  Legend,
  Title,
  BarElement,
  CategoryScale,
  LinearScale,
} from 'chart.js';
import { Pie } from 'vue-chartjs';
import { Bar } from 'vue-chartjs';

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
);

ChartJS.register(ArcElement, Tooltip, Legend);

defineProps<{ name: String }>();

const employeesStore = useEmployeesStore();
const employeesNameFilter = ref<string>('');

onMounted(() => {
  employeesStore.load();
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
  text-align: center;
  margin-top: 25px;
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
