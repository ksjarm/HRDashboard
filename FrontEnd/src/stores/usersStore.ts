import { User } from '@/modules/user';
import { ref } from 'vue';
import { defineStore } from 'pinia';
import useApi, { useApiRawRequest } from '@/modules/api';
import { useNotificationsStore } from '@/stores/notificationsStore';


export const useUsersStore = defineStore('usersStore', () => {
    const apiGetUsers = useApi<User[]>('users');
  const users = ref<User[]>([]);
  let allUsers: User[] = [];

  
  const loadUsers = async () => {
    await apiGetUsers.request();

    if (apiGetUsers.response.value) {
      return apiGetUsers.response.value!;
    }

    return [];
  };
  const load = async () => {
    allUsers = await loadUsers();
    users.value = allUsers;
  };

  const getUserById = (id: number) => {
    return allUsers.find((user) => user.id === id);
  };

  const addUser = async (user: User) => {
    const apiAddUser = useApi<User>('users', {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(user),
    });

    await apiAddUser.request();
    if (apiAddUser.response.value) {
      allUsers.push(apiAddUser.response.value!);
      users.value = allUsers;

      const notificationsStore = useNotificationsStore();
      await notificationsStore.addNotifications({
        message: `User added: ${user.name} ${user.surname}`,
        date: new Date(), 
        type: 'User Added',
      });
    }
  };

  const updateUser = async (user: User) => {
    try {
      const apiUpdateUser = useApi<User>('users/' + user.id, {
        method: 'PUT',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
      });
  
      await apiUpdateUser.request();
  
      if (apiUpdateUser.response.value) {
        const updatedUser = apiUpdateUser.response.value!;
  
        load();
  
        const notificationsStore = useNotificationsStore();
        await notificationsStore.addNotifications({
          message: `User updated: ${updatedUser.name} ${updatedUser.surname}`,
          date: new Date(),
          type: 'User Updated',
        });
      }
    } catch (error) {
      console.error('Error updating user:', error);
      // Handle error as needed
    }
  };
  
  
  const deleteUser = async (user: User) => {
    const deleteUserRequest = useApiRawRequest(`users/${user.id}`, {
      method: 'DELETE',
    });

    const res = await deleteUserRequest();

    if (res.status === 204) {
      let id = allUsers.indexOf(user);

      if (id !== -1) {
        allUsers.splice(id, 1);
      }

      id = users.value.indexOf(user);

      if (id !== -1) {
        users.value.splice(id, 1);
      }
      
      const notificationsStore = useNotificationsStore();
      await notificationsStore.addNotifications({
        message: `User deleted: ${user.name} ${user.surname}`,
        date: new Date(),
        type: 'User Deleted',
      });
    }
  };
  const giveAccess = async (user: User) => {
    try {
      
      // Check if the user already has schedule access
      if (user.permissions === 'schedule_access') {
        console.log('User already has access to the schedule.');
        return;
      }
  
      // Update the user's permissions to schedule_access
      const updatedUser = { ...user, permissions: 'schedule_access' };
      await updateUser(updatedUser);
  
      console.log(`Access granted to user: ${user.name} ${user.surname}`);
    } catch (error) {
      console.error('Error giving access to user:', error);
      // Handle error as needed
    }
  };
  

  const cancelAccess = async (user: User) => {
    // Implement logic to cancel access for the selected user
    const updatedUser = { ...user, permissions: 'schedule_denied' };
    await updateUser(updatedUser);
  };

  return {
    users,
    load,
    getUserById,
    addUser,
    updateUser,
    deleteUser,
    giveAccess,
    cancelAccess,
  };
});
