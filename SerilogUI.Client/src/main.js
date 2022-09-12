import { createApp } from 'vue';
import App from './App.vue';
import MySelect from './components/inputs/MySelect.vue';

import './assets/main.scss';

createApp(App).component('my-select', MySelect).mount('#app');
