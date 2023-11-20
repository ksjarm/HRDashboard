<template>
  <div id="parent">
    <div id="wide" class="">
      <h1 class="label">Employee Profile</h1>
      <h1 class="label">{{ employee?.name }} {{ employee?.surname }}</h1>
      <img src="../assets/profileimg2.png" class="h-50 profileimg" />
      <div>
        <h1 class="role">{{ employee?.position }}</h1>
      </div>
      <div>
        <h1 class="role">Shifts done this month:</h1>
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
  <div>
    <h1 class="upcomingShifts">Upcoming shifts:</h1>
  </div>
  <div id="upcomingShifts">
    <DataTable>
      <Column field="date" header="Date"> </Column>
      <Column field="startTime" header="Start time"> </Column>
      <Column field="endTime" header="End time"> </Column>
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { useEmployeesStore } from '@/stores/employeesStore';
import { onMounted, ref, computed } from 'vue';
import { useRouter } from 'vue-router';

const route = useRouter();
const employeeId = ref<number | null>(null);
const employeesStore = useEmployeesStore();

// Fetch the employee details based on the ID from the route parameters
const employee = computed(() => {
  const id = employeeId.value;
  return id ? employeesStore.getEmployeeById(id) : null;
});

onMounted(() => {
  // Get the employee ID from the route parameters
  const id = route.currentRoute.value.query.id;
  employeeId.value = id ? parseInt(id as string, 10) : null;
});
</script>

<style>
#parent {
  display: flex;
  border: solid;
  border-color: black;
  border-radius: 25px;
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
#upcomingShifts {
  display: flex;
  flex: 1;
}
.upcomingShifts {
  font-size: xx-large;
  margin-right: 20px;
  margin-bottom: 20px;
  margin-top: 20px;
}
.label {
  font-size: xx-large;
  margin-left: 20px;
}
.profileimg {
  margin-top: 70px;
  border: solid;
  border-radius: 25px;
  padding: 10px;
}
.role {
  text-align: center;
  margin-top: 10px;
  font-size: large;
}
.info {
  border: solid;
  border-radius: 25px;
  margin-top: 20px;
  justify-content: center;
  align-items: center;
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
</style>
