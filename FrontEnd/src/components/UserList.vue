<template>
    <div class="min-h-screen bg-gray-50 py-12 px-4 sm:px-6 lg:px-8 text-dark-300">
      <h1 style="text-align: left; margin-bottom: 20px; font-size: 24px; font-weight: bold;">User List</h1>
  
      <table style="width: 100%; border-collapse: collapse;">
        <thead>
          <tr style="background-color: #f2f2f2; text-align: center;">
            <th style="padding: 10px;">Name</th>
            <th style="padding: 10px;">Role</th>
            <th style="padding: 10px;">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in filteredUserList" :key="user.id" style="text-align: center;">
            <td style="padding: 10px;">{{ user.name }} {{ user.surname }}</td>
            <td style="padding: 10px;">{{ user.role }}</td>
            <td style="padding: 10px;">
              <button @click="giveAccess(user)" style="margin-right: 5px; background-color: #3498db; color: #fff; border: none; padding: 8px 16px; border-radius: 5px;">Give Access</button>
              <button @click="cancelAccess(user)" style="background-color: #e74c3c; color: #fff; border: none; padding: 8px 16px; border-radius: 5px;">Cancel Access</button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <div v-if="message" style="margin-top: 20px; padding: 10px; background-color: #d4edda; border: 1px solid #c3e6cb; border-radius: 5px; color: #155724;">
        {{ message }}
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { User } from '@/modules/user';
  import { computed, onMounted, ref } from 'vue';
  import { storeToRefs } from 'pinia';
  import { useUsersStore } from '@/stores/usersStore';
  import { useAuthStore } from '@/stores/authStore';
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  const usersStore = useUsersStore();
  const auth = useAuthStore();
  const { isAuthenticated } = storeToRefs(auth);
  
  const userList = ref<User[]>([]);
  
  const loadUsers = async () => {
    await usersStore.load();
    userList.value = usersStore.users; // Populate the userList with the loaded users
  };
  
  const filteredUserList = computed(() => {
    // Filter out the Staff HR from the userList
    return userList.value.filter(user => user.username !== auth.user?.username);
  });
  
  const user = ref<User | null>(null);
  const message = ref<string>(''); // Reactive variable for feedback message
  
  onMounted(async () => {
    if (!isAuthenticated.value) {
      router.push({ name: 'Log in' });
    }
    await loadUsers();
    user.value = usersStore.users.find((u) => u.username === auth.user?.username) || null;
    console.log('user', user.value);
  });
  
  const giveAccess = (user: User) => {
    
    // Call the giveAccess method from the usersStore
    usersStore.giveAccess(user);
    message.value = 'Access granted!';
  };
  
  const cancelAccess = (user: User) => {
    // Call the cancelAccess method from the usersStore
    usersStore.cancelAccess(user);
    message.value = 'Access canceled!';
  };
  </script>
  