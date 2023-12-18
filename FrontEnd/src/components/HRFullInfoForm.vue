<template>
    <div id="parent">
      <div id="wide" class="">
        <h1 class="infoLabel">Your Profile</h1>
        <h1 class="label">{{ user?.name }} {{ user?.surname }}</h1>
        <h1 class="infoLabel">Role: {{ user?.role }}</h1>
        <img src="../assets/user-profile-image.jpg" class="h-50 profileimgHR" />
      </div>
      <div id="narrowHR" class="">
        <div class="infoHR">
          <h1 class="infoLabel"><span style="font-weight: bold;">Name: </span>{{ user?.name }}</h1>
        </div>
        <div class="infoHR">
          <h1 class="infoLabel"><span style="font-weight: bold;">Surname: </span>{{ user?.surname }}</h1>
        </div>
        <div class="infoHR">
          <h1 class="infoLabel"><span style="font-weight: bold;">Phone Number: </span>{{ user?.phoneNumber }}</h1>
        </div>
        <div class="infoHR">
          <h1 class="infoLabel"><span style="font-weight: bold;">Address: </span>{{ user?.adress }}</h1>
        </div>
        <div class="infoHR">
          <h1 class="infoLabel"><span style="font-weight: bold;">Username: </span>{{ user?.username }}</h1>
        </div>
        <div class="finalinfo"></div>
        <div>
          <button v-if="user?.role === 'Staff HR'" @click="showUserList" class="btn-blue">
    Give Access to User
  </button>
      </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
import { onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores/authStore';
import { useUsersStore } from '@/stores/usersStore';
import { useRouter } from 'vue-router';
import { User } from '@/modules/user';

const router = useRouter();
const auth = useAuthStore();
const { isAuthenticated } = storeToRefs(auth);

const usersStore = useUsersStore();

const user = ref<User | null>(null);

onMounted(async () => {
  if (!isAuthenticated.value) {
    router.push({ name: 'Log in' });
  }
  await usersStore.load();
  user.value = usersStore.users.find((u) => u.username === auth.user?.username) || null;
  console.log('user', user.value)
});

const showUserList = () => {
  router.push({ name: 'User list' }); // Assuming you have a route named 'UserList'
};
  </script>
  
  <style>
  #narrowHR {
    width: 650px;
    margin-bottom: 20px;
    /* Just so it's visible */
  }
  .profileimgHR {
    margin-top: 30px;
    margin-left: 10px;
    border: solid;
    border-radius: 25px;
    border-color: rgba(199, 210, 254, 0.294);
    border-width: 8px 8px 8px 8px;
    padding: 10px;
  }
  .infoHR {
    display: flex;
    border: solid;
    border-color: rgba(199, 210, 254, 0.294);
    background-color: rgba(199, 210, 254, 0.294);
    border-radius: 25px;
    margin-top: 30px;
    margin-right: 60px;
    justify-content: left;
    align-items: left;
  }
  .btn-blue {
  background-color: #3498db;
  color: #fff;
  border: none;
  padding: 8px 16px;
  border-radius: 5px;
  margin-right: 5px;
}
  </style>
  