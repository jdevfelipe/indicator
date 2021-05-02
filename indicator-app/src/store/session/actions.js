export function login ({commit}, payload) {
    commit('SET_INDICATOR', payload)
}

export function logout ({commit}) {
    commit('SET_INDICATOR', {})
}

