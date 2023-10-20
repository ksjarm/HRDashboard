<template>
  <div class="flex h-screen bg-gray-800">
    <nav class="w-48 bg-gray-800 flex flex-col justify-center items-center">
      <router-link
        to="/"
        class="text-gray-300 hover:bg-gray-700 hover:text-white px-4 py-2 text-sm font-medium"
        active-class="bg-gray-900 text-white"
      >
        Dashboard
      </router-link>
      <router-link
        to="/newexercise"
        class="text-gray-300 hover:bg-gray-700 hover:text-white px-4 py-2 text-sm font-medium"
        active-class="bg-gray-900 text-white"
      >
        Employees
      </router-link>
      <router-link
        to="/"
        class="text-gray-300 hover:bg-gray-700 hover:text-white px-4 py-2 text-sm font-medium"
        active-class="bg-gray-900 text-white"
      >
        Shedule
      </router-link>
    </nav>
    <div class="flex-1 p-8">
      <router-view />
    </div>
    
  </div>
</template>

<script setup lang="ts"></script>

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
</style>
