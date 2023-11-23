<template>
    <div id="parent1">
      <div id="wide1" class="">
        <h1 class="label1">Your Profile</h1>
        <h1 class="label1">{{ user?.name }} {{ user?.surname }}</h1>
        <img src="../assets/user-profile-image.jpg" class="h-50 profileimg1" />
        <!-- <div>
          <h1 class="role">{{ user?.position }}</h1>
        </div> -->
        <div>
          <h1 class="role1">Shifts done this month:</h1>
        </div>
      </div>
      <div id="narrow1" class="">
        <div class="info1">
          <h1 class="infoLabel1">Name: {{ user?.name }}</h1>
        </div>
        <div class="info1">
          <h1 class="infoLabel1">Surname: {{ user?.surname }}</h1>
        </div>
        <div class="info1">
          <h1 class="infoLabel1">Gender: {{ user?.gender }}</h1>
        </div>
        <div class="info1">
          <h1 class="infoLabel1">Date of Birth: {{ user?.dateOfBirth }}</h1>
        </div>
        <div class="info1">
          <h1 class="infoLabel1">Email: {{ user?.email }}</h1>
        </div>
        <div class="info1">
          <h1 class="infoLabel1">Phone number: {{ user?.phoneNumber }}</h1>
        </div>
        <div class="finalinfo1"></div>
      </div>
    </div>
    <div>
      <h1 class="upcomingShifts1">Upcoming shifts:</h1>
    </div>
    <div id="upcomingShifts1">
      <DataTable>
        <Column field="date" header="Date"> </Column>
        <Column field="startTime" header="Start time"> </Column>
        <Column field="endTime" header="End time"> </Column>
      </DataTable>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useUsersStore } from '@/stores/usersStore';
  import { onMounted, ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  
  const route = useRouter();
  const userId = ref<number | null>(null);
  const usersStore = useUsersStore();
  
  const user = computed(() => {
    const id = userId.value;
    return id ? usersStore.getUserById(id) : null;
  });
  
  onMounted(() => {
    const id = route.currentRoute.value.query.id;
    userId.value = id ? parseInt(id as string, 10) : null;
  });
  </script>
  
  <style>
  #parent1 {
    display: flex;
    border: solid;
    border-color: rgba(199, 210, 254);
    border-radius: 25px;
  }
  #narrow1 {
    width: 500px;
    /* Just so it's visible */
  }
  #wide1 {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
  #upcomingShifts1 {
    display: flex;
    flex: 1;
  }
  .upcomingShifts1 {
    font-size: xx-large;
    margin-right: 20px;
    margin-bottom: 20px;
    margin-top: 20px;
  }
  .label1 {
    font-size: xx-large;
    margin-left: 20px;
  }
  .profileimg1 {
    margin-top: 70px;
    border: solid;
    border-radius: 25px;
    border-color: rgba(199, 210, 254);
    padding: 10px;
  }
  .role1 {
    text-align: center;
    margin-top: 10px;
    font-size: large;
  }
  .info1 {
    border: solid;
    border-color: rgba(199, 210, 254);
    border-radius: 25px;
    margin-top: 20px;
    justify-content: center;
    align-items: center;
    width: 400px;
  }
  .finalinfo1 {
    margin-top: 20px;
    justify-content: center;
    align-items: center;
    width: 400px;
  }
  .infoLabel1 {
    font-size: x-large;
    margin-left: 10px;
  }
  </style>
  