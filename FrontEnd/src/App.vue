<template>
  <nav class="bg-purple-800 p-5 text-white flex items-center">
    <div class="text-3xl text-purple-600 font-bold ml-4">HR Dashboard</div>
    <div class="bg-white rounded-full p-2 flex items-center ml-4">
      <input
        type="text"
        class="bg-transparent outline-none w-110 pl-2 text-gray-500"
        placeholder="Search"
      />
    </div>
    <div class="flex items-center">
      <div class="bg-white rounded-full p-2 ml-260">
        <img
          src="./assets/user-profile-image.jpg"
          alt="User Profile"
          class="h-8 w-8 cursor-pointer"
          @click="redirectToAccountSettings"
        />
      </div>
    </div>
  </nav>
  <div class="flex h-screen bg-white-800">
    <nav class="w-65 bg-purple-800 flex flex-col justify-col items-start">
      <router-link
        to="/"
        class="text-dark-500 hover:bg-dark-700 hover:text-purple-500 px-12 ml-4 py-4 mb-4 text-lg font-medium border-white border-2 rounded-full"
        active-class="bg-white-900 text-white"
      >
        D a s h b o a r d
      </router-link>
      <router-link
        to="/newexercise"
        class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-12 py-4 ml-4 mb-4 text-lg font-medium border-white border-2 rounded-full"
        active-class="bg-white-900 text-white"
      >
        E m p l o y e e s
      </router-link>
      <router-link
        to="/shedule"
        class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-14 py-4 mb-4 ml-4 text-lg font-medium border-white border-2 rounded-full"
        active-class="bg-white-900 text-white"
      >
        S c h e d u l e
      </router-link>
      <router-link
        to="/notifications"
        class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-8 py-4 mb-4 ml-4 text-lg font-medium border-white border-2 rounded-full"
        active-class="bg-white-900 text-white"
      >
        N o t i f i c a t i o n s
      </router-link>
      <router-link
        to="/login"
        class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-20 py-4 mb-4 ml-4 text-lg font-medium border-white border-2 rounded-full"
        active-class="bg-white-900 text-white"
      >
        L o g i n
      </router-link>
    </nav>
    <div class="flex-1 p-8">
      <!-- <router-view /> -->
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
const router = useRouter();

const redirectToAccountSettings = () => {
  router.push('/newexercise'); // Adjust the route as needed
};
</script>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
.bg-purple-800 {
  background-color: #ccccff;
}
</style>
