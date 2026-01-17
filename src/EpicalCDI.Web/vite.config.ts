import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: true,
    proxy: {
        '/api': {
            target: process.env.services__api__https__0 || process.env.services__api__http__0,
            changeOrigin: true,
            secure: false
        }
    }
  }
})