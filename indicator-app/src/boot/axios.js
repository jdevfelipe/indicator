import axios from 'axios'

const http = axios.create({
  baseURL: process.env.API,
  headers: {
    post: {
      'Content-Type': 'application/json'
    }
  }
})

export { http }