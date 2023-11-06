<template>
  <div class="container">
    <div class="div1">
      <div class="div1Text">Welcome to HR Dashboard!👋</div>
    </div>
    <div class="div2">
      <div class="div2Text">EmployeeStat1</div>
    </div>
    <div class="div2">
      <div class="div2Text">Schedule</div>
      <div class="schedule">
        <div class="today-date">{{ today.toLocaleDateString() }}</div>
        <div class="shifts">
          <div v-for="shift in todayShifts" :key="shift.id">
            Employee Name: {{ shift.startTime }} - {{ shift.endTime }} ({{ shift.title }})
          </div>
          <div v-if="todayShifts.length === 0">No shifts for today.</div>
        </div>
      </div>
    </div>
    <div class="div2">
      <div class="div2Text">EmployeeStat2</div>
    </div>
    <div class="div2">
      <div class="div2Text">Notifications</div>

      <div class="notification-block" v-for="notification in lastTwoNotifications" :key="notification.notificationId">
        <div>{{notification.date}}  {{ notification.type }}</div>
      </div>
    </div>   
  </div>
</template>

<script setup lang="ts">
import { useEmployeesStore } from '@/stores/employeesStore';
import { useShiftsStore } from '@/stores/shiftsStore';
import { useNotificationsStore } from '@/stores/notificationsStore';
import { onMounted, ref, watch, computed } from 'vue';

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
  return notificationsStore.notifications.slice(-2).reverse(); 
});

onMounted(() => {
  employeesStore.load();
  shiftsStore.load();
  notificationsStore.load();
});

watch(employeesNameFilter, (name) => {
  employeesStore.filterEmployeesByName(name);
});

</script>

<style scoped>
.container {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  grid-gap: 10px;
}

.div1 {
  grid-column: 1 / 4;
  background-color: white;
  border: solid;
  border-radius: 25px;
  border-color: #ccccff;
  height: 100px;
  font-size: x-large;
  font-weight: bold;
}
.div6 {
  grid-column: 1 / 4;
  background-color: white;
  border: solid;
  border-radius: 25px;
  border-color: #ccccff;
  height: 100px;
  font-size: x-large;
  font-weight: bold;
  margin-top: 50px;
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
  height: 180px;
  font-size: x-large;
  font-weight: bold;
}
.div2Text {
  margin-left: 10px;
}
.div3,
.div4,
.div5 {
  background-color: #3399ff;
  height: 100px;
}
.today-date {
  background-color:rgba(199, 210, 254); 
  text-align: center; 
  font-size: 150%;
}
.shifts {
  font-weight: normal;
  margin-left: 20px;
}
.notification-block{
  background-color: #f0f0f0;
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
