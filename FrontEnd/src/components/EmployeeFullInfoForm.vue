<template>
  <div id="parent">
    <div id="wide" class="">
      <h1 class="infoLabel">Employee Profile</h1>
      <h1 class="label">{{ employee?.name }} {{ employee?.surname }}</h1>
      <img
        v-if="employee?.name == 'Erik'"
        src="../assets/Capture.png"
        class="h-50 profileimg"
      />
      <img
        v-if="employee?.name == 'Rene'"
        src="../assets/Capture2.png"
        class="h-50 profileimg"
      />
      <img
        v-if="employee?.name == 'Mari'"
        src="../assets/Capture3.png"
        class="h-50 profileimg"
      />
      <div>
        <h1 class="role">{{ employee?.position }}</h1>
      </div>
      <div>
        <h1 class="role">Shifts done: {{ pastFilteredShifts }}</h1>
      </div>
    </div>
    <div id="narrow" class="">
      <div class="info">
        <h1 class="infoLabel">Name: {{ employee?.name }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Surname: {{ employee?.surname }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Gender: {{ employee?.gender }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Date of Birth: {{ employee?.dateOfBirth }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Email: {{ employee?.email }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Phone number: {{ employee?.phoneNumber }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Address: {{ employee?.adress }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Salary: {{ employee?.salary }}</h1>
      </div>
      <div class="info">
        <h1 class="infoLabel">Status: {{ employee?.status }}</h1>
      </div>
      <div class="finalinfo"></div>
    </div>
  </div>
  <div class="text-center mt-5 ml-5 mr-5">
    <h1 class="text-left mb-5 font-bold sizeFont">Upcoming Shifts</h1>
    <DataTable :value="futureFilteredShifts">
      <Column field="title" header="Title" />
      <Column field="date" header="Date" />
      <Column field="startTime" header="Start time" />
      <Column field="endTime" header="End Time"></Column>
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { useEmployeeShiftsStore } from '@/stores/employeeShiftsStore';
import { useEmployeesStore } from '@/stores/employeesStore';
import { useShiftsStore } from '@/stores/shiftsStore';
import { onMounted, ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores/authStore';

const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);

const route = useRouter();
const employeeId = ref<number>(1);
const employeesStore = useEmployeesStore();
const employeesShiftsStore = useEmployeeShiftsStore();
const { employeeShifts } = storeToRefs(employeesShiftsStore);
const shiftsStore = useShiftsStore();

const { shifts } = storeToRefs(shiftsStore);
onMounted(async () => {
  if (!isAuthenticated.value) {
    route.push({ name: 'Log in' });
  }
  await shiftsStore.load();
  await employeesShiftsStore.load();
  const id = route.currentRoute.value.query.id;
  employeeId.value = parseInt(id as string, 10);
  if (id) {
    await employeesStore.getEmployeeShiftsById(employeeId.value);
  }
});

const employee = computed(() => {
  const id = employeeId.value;
  return id ? employeesStore.getEmployeeById(id) : null;
});

const totalShifts = computed(() => {
  console.log('bb' + employeesStore.getEmployeeShiftsById(employeeId.value));
  return employeesStore.getEmployeeShiftsById(employeeId.value);
});
/* const shiftsDone = computed(() => {
  if (employeeId.value) {
    return employeeShifts.value.filter((shift) => {
      return shift.employeeId === employeeId.value;
    }).length;
  }

  return 0;
}); */

const futureFilteredShifts = computed(() => {
  const id = employeeId.value;
  const today = new Date().toISOString().split('T')[0]; // Get current date in YYYY-MM-DD format

  if (id && shifts.value) {
    return shifts.value.filter((shift) => {
      return (
        shift.employeeIds &&
        shift.employeeIds.includes(id) &&
        shift.date >= today
      );
    });
  }
  return [];
});
const pastFilteredShifts = computed(() => {
  const id = employeeId.value;
  const today = new Date().toISOString().split('T')[0]; // Get current date in YYYY-MM-DD format

  if (id && shifts.value) {
    return shifts.value.filter((shift) => {
      return (
        shift.employeeIds &&
        shift.employeeIds.includes(id) &&
        shift.date < today
      );
    }).length;
  }
  return [];
});
</script>

<style>
#parent {
  display: flex;
  border: solid;
  border-color: #ccccff;
  border-radius: 25px;
  margin-left: 30px;
  margin-right: 30px;
  margin-top: 20px;
}
#narrow {
  width: 500px;
  /* Just so it's visible */
}
#wide {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.upcomingShifts {
  font-size: xx-large;
  margin-right: 20px;
  margin-bottom: 20px;
  margin-top: 20px;
  margin-left: 20px;
}
.label {
  font-size: xx-large;
  font-weight: bold;
  margin-left: 20px;
}
.profileimg {
  margin-top: 45px;
  margin-bottom: 15px;
  border: solid;
  border-radius: 25px;
  padding: 10px;
}
.role {
  text-align: center;
  margin-top: 10px;
  font-size: x-large;
}
.info {
  display: flex;
  border: solid;
  border-radius: 25px;
  margin-top: 20px;
  justify-content: left;
  align-items: left;
  width: 400px;
}
.finalinfo {
  margin-top: 20px;
  justify-content: center;
  align-items: center;
  width: 400px;
}
.infoLabel {
  font-size: x-large;
  margin-left: 10px;
}
.sizeFont {
  font-size: xx-large;
}
</style>
