<template>
  <div>
    <nav class="bg-indigo-200 p-5 text-white flex items-center">
      <div class="text-3xl text-dark-600 font-bold ml-1 flex">HR Dashboard</div>
      <div class="bg-white p-2 relative ml-4 flex-grow rounded-full">
        <div class="bg-white rounded-full p-2 flex items-center">
          <input
            v-model="searchQuery"
            type="text"
            class="bg-transparent outline-none w-full pl-2 text-gray-500"
            placeholder="Search"
            v-if="isAuthenticated"
            @input="searchEmployees"
          />
        </div>
        <transition name="fade">
          <div v-if="searchResults.length > 0" class="search-results">
            <div
              v-for="result in searchResults"
              :key="result.id"
              class="border border-solid border-black p-2 flex items-center relative"
            >
              <img
                v-if="result.name === 'Erik'"
                src="./assets/Capture.png"
                alt="Male Photo"
                class="h-12 w-12 rounded-full mr-2"
              />
              <img
                v-else-if="result.name === 'Rene'"
                src="./assets/Capture2.png"
                alt="Female Photo"
                class="h-12 w-12 rounded-full mr-2"
              />
              <img
                v-else-if="result.name === 'Mari'"
                src="./assets/Capture3.png"
                alt="Female Photo"
                class="h-12 w-12 rounded-full mr-2"
              />
              <!-- Add images for other conditions -->
              <div class="flex flex-col">
                <p class="text-black font-bold">{{ result.name }}</p>
                <p class="text-black">{{ result.surname }}</p>
              </div>
              <p class="ml-auto text-black">{{ result.position }}</p>

              <!-- Button on the right side -->
              <button
                @click="showEmployeeDetails(result)"
                class="ml-2 p-2 bg-green-300 text-white rounded-full"
              >
                Show Details
              </button>
            </div>
          </div>
          <div
            v-else-if="searchResults.length === 0 && searchQuery !== ''"
            class="search-results"
          >
            <p>No results found</p>
          </div>
        </transition>
      </div>
      <div class="flex items-center">
        <div class="bg-white rounded-full p-2 ml-4">
          <img
            src="./assets/user-profile-image.jpg"
            alt="User Profile"
            class="h-8 w-8 cursor-pointer"
            v-if="isAuthenticated"
            @click="redirectToAccountSettings"
          />
        </div>
      </div>
    </nav>
    <div class="flex">
      <nav
        class="w-14 sm:w-60 lg:h-190 xl:h-200 bg-indigo-200 flex flex-col justify-start items-start"
      >
        <router-link
          to="/dashboard"
          v-if="isAuthenticated"
          class="text-dark-500 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-12 py-2 sm:py-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full flex"
          active-class="bg-white-900 text-white"
        >
          Dashboard
        </router-link>
        <router-link
          to="/employees"
          v-if="isAuthenticated"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-12 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Employees
        </router-link>
        <router-link
          to="/shedule"
          v-if="isAuthenticated"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-14 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Schedule
        </router-link>
        <router-link
          to="/notifications"
          v-if="isAuthenticated"
          class="text-dark-300 hover-bg-dark-700 hover:text-purple-500 px-3 sm:px-10 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Notifications
        </router-link>
        <router-link
          to="/"
          v-if="isAuthenticated"
          @click="signOut"
          class="text-dark-300 hover:bg-dark-700 hover:text-purple-500 px-3 sm:px-17 py-2 sm:py-4 ml-4 sm:ml-3 mb-4 sm:text-lg text-base font-medium border-white border-2 rounded-full"
          active-class="bg-white-900 text-white"
        >
          Logout
        </router-link>
      </nav>
      <div class="flex-1">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { ref, onMounted, onUnmounted, watch } from 'vue';
import { useAuthStore } from './stores/authStore';
import { storeToRefs } from 'pinia';
import { useEmployeesStore } from '@/stores/employeesStore';
import { Employee } from '@/modules/employee';

const employeesStore = useEmployeesStore();
const { employees } = storeToRefs(employeesStore);

const authStore = useAuthStore();
const { logout } = authStore;
const { isAuthenticated } = storeToRefs(authStore);

const signOut = () => {
  logout();
  router.push({ name: 'Log in' });
};

const router = useRouter();
const secondNavClass = ref('sm:w-60 sm:h-250'); // Default class for second nav

const redirectToAccountSettings = () => {
  router.push('/hrfullinfo'); // Adjust the route as needed
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
const searchQuery = ref('');
const searchResults = ref([]);

const searchEmployees = () => {
  const query = searchQuery.value.toLowerCase();
  if (query === '') {
    searchResults.value = [];
  } else {
    searchResults.value = employees.value.filter((employee) =>
      employee.name.toLowerCase().includes(query),
    );
  }
};
const showEmployeeDetails = (employee: Employee) => {
  searchQuery.value = '';
  router.push({
    name: 'Get full info',
    query: { id: Number(employee.id) },
  });
};

watch(searchQuery, () => {
  searchEmployees();
});
</script>

<style scoped>
/* Inside the style section of App.vue */
.search-results {
  position: absolute;
  width: 100%;
  background-color: white;
  box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
  z-index: 1;
  margin-top: 10px;
  max-height: 200px; /* Set a maximum height for the dropdown */
  overflow-y: auto; /* Enable vertical scrolling if needed */
  border: 1px solid black; /* Add a border for better visibility */
  margin-left: -7px;
}

.appstart {
  height: 100vh;
}
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
