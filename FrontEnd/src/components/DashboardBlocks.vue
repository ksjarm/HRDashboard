<template>
  <div class="container">
    <div class="div1">
      <div class="div1Text">Welcome to HR Dashboard!👋</div>
    </div>
    <div class="div2">
      <div class="div2Text">EmployeeStat1</div>
    </div>
    <div class="div2">
      <div class="div2Text">Scheldule</div>
    </div>
    <div class="div2">
      <div class="div2Text">EmployeeStat2</div>
    </div>
    <div class="div2"><div class="div2Text">Notifications</div></div>
    <div class="div6">
      <div class="div1Text">Smth else</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useEmployeesStore } from '@/stores/employeesStore';
import { onMounted, ref, watch } from 'vue';

defineProps<{ name: String }>();

const employeesStore = useEmployeesStore();
const employeesNameFilter = ref<string>('');

onMounted(() => {
  employeesStore.load();
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
  height: 100px;
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
