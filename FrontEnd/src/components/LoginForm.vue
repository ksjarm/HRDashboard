<template>
  <div class="min-h-full flex bg-indigo-200">
    <div class="flex-1 flex items-center justify-center">
      <!-- Add a container div to control the size of the second div -->
      <div
        class="max-w-2xl w-70vw h-115 bg-white p-8 rounded-md shadow-md mr-40 marginup"
      >
        <div class="space-y-8">
          <div>
            <h1 class="text-3xl font-bold mb-4 text-black">Welcome back!</h1>
            <h1 class="text-3xl font-bold mb-4">Please log in to continue</h1>
            <p v-if="showError" class="text-red-400">
              Invalid login or password
            </p>
          </div>
          <form @submit.prevent="submit">
            <div class="rounded-md shadow-sm space-y-6">
              <div>
                <label for="username">Login:</label>
                <input
                  type="text"
                  v-model="user.username"
                  :class="{ 'border-red-500': showError }"
                  name="username"
                  class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                />
              </div>
              <div>
                <label for="password">Password:</label>
                <input
                  type="password"
                  v-model="user.password"
                  :class="{ 'border-red-500': showError }"
                  name="password"
                  class="appearance-none rounded-md relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                />
              </div>
              <div class="flex items-center">
                <input
                  type="checkbox"
                  id="rememberMe"
                  v-model="user.rememberMe"
                  class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                />
                <label
                  for="rememberMe"
                  class="ml-2 block text-sm text-gray-900"
                >
                  Remember me
                </label>
              </div>

              <div>
                <button
                  class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-400 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                  type="submit"
                >
                  <span
                    class="absolute left-0 inset-y-0 flex items-center pl-3"
                  >
                  </span>
                  Log in
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { User } from '@/modules/user';
import router from '@/router/router';
import { useAuthStore } from '@/stores/authStore';
import { ref, watch, onMounted } from 'vue';
import { storeToRefs } from 'pinia';

const auth = useAuthStore();
const user: User = { username: '', password: '', rememberMe: false };
const { isAuthenticated } = storeToRefs(auth);

let showError = ref(false);
let showWelcome = ref(true);

watch(
  () => showError.value,
  (newValue) => {
    if (newValue) {
      showWelcome.value = false;
    }
  },
);

onMounted(() => {
  if (user.rememberMe) {
    const storedUsername = localStorage.getItem('rememberedUsername');
    const storedPassword = localStorage.getItem('rememberedPassword');
    if (storedUsername && storedPassword) {
      user.username = storedUsername;
      user.password = storedPassword;
    }
  }
});

const submit = async () => {
  showError.value = !(await auth.login(user));
  if (user.rememberMe) {
    localStorage.setItem('rememberedUsername', user.username);
    localStorage.setItem('rememberedPassword', user.password);
  }
  if (isAuthenticated.value) {
    console.log(isAuthenticated.value);
    router.push({ name: 'Dashboard' });
  } else {
  }
};
</script>
<style>
.marginup {
  margin-top: -250px;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.popup-enter,
.popup-leave-to {
  opacity: 0;
  transform: scale(0.8);
}
.popup-enter-active,
.popup-leave-active {
  transition:
    opacity 0.5s,
    transform 0.5s;
}
</style>
