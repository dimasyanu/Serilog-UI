<template>
  <div id="pagination-select">
    <div
      class="btn pagination-btn mx-2"
      :class="{
        disabled: currentPage <= 1,
        'btn-outline-primary': currentPage > 1,
        'btn-outline-secondary': currentPage <= 1,
      }"
      @click="goToPage(currentPage - 1)"
    >
      Prev
    </div>
    <input
      class="form-control inline"
      list="pagination-options"
      id="pagination-list"
      placeholder="Page"
      v-model="pageInput"
      @keydown="returnKeydown($event)"
    />
    <label class="ms-2"> / {{ totalPages }}</label>
    <datalist id="pagination-options">
      <option v-for="i in totalPages" :value="i" :key="i"></option>
    </datalist>
    <div
      class="btn pagination-btn mx-2"
      :class="{
        disabled: currentPage >= totalPages,
        'btn-outline-primary': currentPage < totalPages,
        'btn-outline-secondary': currentPage >= totalPages,
      }"
      @click="goToPage(currentPage + 1)"
    >
      Next
    </div>
  </div>
</template>

<script>
export default {
  props: {
    totalItems: {
      type: Number,
      default: 0,
    },
    startIndex: {
      type: Number,
      default: 0,
    },
    pageSize: {
      type: Number,
      default: 0,
    },
  },
  computed: {
    currentPage() {
      if (this.pageSize < 1) return 1;
      return this.startIndex / this.pageSize + 1;
    },
    totalPages() {
      if (this.pageSize === 0) return 0;
      if (this.pageSize < 0) return 1;
      return Math.ceil(this.totalItems / this.pageSize);
    },
  },
  data() {
    return { pageInput: null };
  },
  methods: {
    goToPage(pageNum) {
      if (pageNum < 1 || pageNum > this.totalPages) return;
      this.$emit('goToPage', (pageNum - 1) * this.pageSize);
    },
    returnKeydown(event) {
      if (event.key != 'Enter' || this.pageInput < 1 || this.pageInput > this.totalPages) return;
      this.goToPage(this.pageInput);
    },
  },
  mounted() {
    if (this.pageInput == null) {
      this.pageInput = this.currentPage;
    }
  },
  watch: {
    startIndex() {
      this.pageInput = this.currentPage;
    },
  },
};
</script>
<style lang="scss" scoped>
.inline {
  display: inline;
}
#pagination-select {
  input {
    width: 6rem;
  }

  #pagination-list {
    text-align: center;
  }
}
.pagination-btn {
  margin-top: -6px;
  --bs-btn-padding-y: 0.3rem;
  --bs-btn-padding-x: 0.8rem;
}
</style>
