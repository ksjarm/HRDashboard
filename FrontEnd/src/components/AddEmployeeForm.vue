<template>
  <div class="flex justify-center labelDiv bg-indigo-200 mt-2 ml-2 mr-2">
    Add Employee
  </div>
  <div
    class="min-h-full flex items-center justify-center py-12 px-4 sm:px-6 lg:px-6 minusmargin"
  >
    <form class="max-w-md w-full space-y-8">
      <div class="space-y-6">
        <div class="rounded-md shadow-sm -space-y-px">
          <div>
            <label for="name">Name</label>
            <input
              id="name"
              name="name"
              v-model="props.employee.name"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Name"
            />
          </div>
          <div>
            <label for="surname">Surname</label>
            <input
              id="surname"
              name="surname"
              v-model="props.employee.surname"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Surname"
            />
          </div>
          <div>
            <label for="gender">Gender</label>
            <select
              id="gender"
              name="gender"
              v-model="props.employee.gender"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            >
              <option value="1">Male</option>
              <option value="2">Female</option>
            </select>
          </div>
          <div>
            <label for="dateOfBirth">Date of birth</label>
            <input
              id="dateOfBirth"
              type="date"
              name="dateOfBirth"
              v-model="props.employee.dateOfBirth"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            />
          </div>
          <div>
            <label for="email">Email</label>
            <input
              id="email"
              name="email"
              v-model="props.employee.email"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            />
          </div>
          <div>
            <label for="phoneNumber">Phone number</label>
            <input
              id="phoneNumber"
              name="phoneNumber"
              v-model="props.employee.phoneNumber"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            />
          </div>
          <div>
            <label for="adress">Adress</label>
            <input
              id="adress"
              name="adress"
              v-model="props.employee.adress"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            />
          </div>
          <div>
            <label for="position">Position</label>
            <select
              id="position"
              name="position"
              v-model="props.employee.position"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            >
              <option>Vendor</option>
              <option>Shift Manager</option>
              <option>Warehouse worker</option>
              <option>Accountant</option>
              <option>Consultant</option>
            </select>
          </div>
          <div>
            <label for="salary">Salary</label>
            <input
              id="salary"
              type="number"
              name="salary"
              v-model="props.employee.salary"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            />
          </div>
          <div>
            <label for="status">Status</label>
            <select
              id="status"
              name="status"
              v-model="props.employee.status"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
            >
              <option value="1">Active</option>
              <option value="2">On vacation</option>
              <option value="3">On Maternity Leave</option>
              <option value="4">On Leave process</option>
            </select>
          </div>
        </div>

        <div>
          <button
            @click.prevent="submitForm"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-200 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          >
            <span class="absolute left-0 inset-y-0 flex items-center pl-3">
            </span>
            Add employee
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { Employee } from '@/modules/employee';
import { useEmployeesStore } from '@/stores/employeesStore';
import { useRouter } from 'vue-router';
import { onMounted } from 'vue';
import { useAuthStore } from '@/stores/authStore';
import { storeToRefs } from 'pinia';

const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);

onMounted(() => {
  if (!isAuthenticated.value) {
    router.push({ name: 'Log in' });
  }
});

const props = defineProps<{ employee: Employee }>();

const { addEmployee, updateEmployee } = useEmployeesStore();
const router = useRouter();

const submitForm = () => {
  if (props.employee.id === 0) {
    addEmployee(props.employee);
  } else {
    updateEmployee(props.employee);
  }

  props.employee.name = '';
  props.employee.surname = '';
  props.employee.gender = 'Male';
  props.employee.dateOfBirth = '';
  props.employee.adress = '';
  props.employee.phoneNumber = '';
  props.employee.position = '';
  props.employee.salary = 0;
  props.employee.status = 'Active';

  router.push({ name: 'Employees' });
};
</script>

<style>
.addemployeeLabel {
  font-size: xx-large;
  justify-self: center;
  display: flex;
  align-items: center;
}
.minusmargin {
  margin-top: -30px;
}
.labelDiv {
  font-size: xx-large;
  border: solid;
  border-color: black;
  border-radius: 25px;
}
</style>
