import Vue from 'vue'
import Vuex from 'vuex'
import session from './session'
import createPersistedState from 'vuex-persistedstate'

// import example from './module-example'

Vue.use(Vuex)

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Store instance.
 */

export default function (/* { ssrContext } */) {
  const Store = new Vuex.Store({
    modules: {
      session
    },

    // enable strict mode (adds overhead!)
    // for dev mode only
    plugins: [createPersistedState()],
    strict: process.env.DEV
  })

  return Store
}
