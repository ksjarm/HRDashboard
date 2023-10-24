<template>
  <div>
    <nav class="bg-purple-800 p-5 text-white flex items-center">
      <div class="text-3xl text-purple-600 font-bold ml-1 flex">
        HR Dashboard
      </div>
      <div class="bg-white rounded-full p-2 flex items-center ml-4 flex-grow">
        <input
          type="text"
          class="bg-transparent outline-none w-full pl-2 text-gray-500"
          placeholder="Search"
        />
      </div>
      <div class="flex items-center">
        <div class="bg-white rounded-full p-2 ml-4">
          <img
            src="./assets/user-profile-image.jpg"
            alt="User Profile"
            class="h-8 w-8 cursor-pointer"
            @click="redirectToAccountSettings"
          />
        </div>
      </div>
    </nav>
    <div class="flex">
      <nav
        class="w-14 sm:w-60 sm:h-250 bg-purple-800 flex flex-col justify-start items-start"
      >
        <router-link
          to="/"
          class="text-dark-500 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-12 py-2 sm:py-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full flex"
          active-class="bg-white-900 text-white"
        >
          Dashboard
        </router-link>
        <router-link
          to="/newexercise"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-12 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Employees
        </router-link>
        <router-link
          to="/schedule"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-14 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Schedule
        </router-link>
        <router-link
          to="/notifications"
          class="text-dark-300 hover-bg-dark-700 hover:text-purple-500 px-3 sm:px-10 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Notifications
        </router-link>
        <router-link
          to="/exercises"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-17 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Login
        </router-link>
      </nav>
      <div class="flex-1 p-8">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { ref, onMounted, onUnmounted } from 'vue';

const router = useRouter();
const secondNavClass = ref('sm:w-60 sm:h-250'); // Default class for second nav

const redirectToAccountSettings = () => {
  router.push('/newexercise'); // Adjust the route as needed
};
const updateSecondNavClass = () => {
  if (window.innerWidth < 640) {
    secondNavClass.value = 'sm:w-40 sm:h-200'; // Adjust the classes as needed
  } else {
    secondNavClass.value = 'sm:w-60 sm:h-250'; // Default classes for larger screens
  }
};

// Watch the window size and update the class
onMounted(() => {
  updateSecondNavClass();
  window.addEventListener('resize', updateSecondNavClass);
});

onUnmounted(() => {
  window.removeEventListener('resize', updateSecondNavClass);
});
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
.borderR {
  border-radius: 25px;
}
</style>
