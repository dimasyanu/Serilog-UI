<template>
  <select v-model="value">
    <option v-for="(item, i) in options" :value="item" :key="i" :selected="modelValue == item ? 'selected' : ''">
      {{ item }}
    </option>
  </select>
</template>
<script>
export default {
  props: {
    options: {
      default: () => [],
      type: Array,
    },
    modelValue: [Number, String],
  },
  data() {
    return {
      value: null,
    };
  },
  watch: {
    modelValue(val) {
      if (val == this.value) return;
      this.value = val;
    },
    value(val) {
      this.$emit('update:modelValue', val);
    },
  },
};
</script>
