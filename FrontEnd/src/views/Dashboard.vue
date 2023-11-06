<template>
  <DashboardBlocks :name="name" />
</template>

<script setup lang="ts">
import DashboardBlocks from '@/components/DashboardBlocks.vue';

defineProps<{ name: String }>();
</script>
