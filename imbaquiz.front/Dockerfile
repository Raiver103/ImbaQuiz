# Stage 1: build
FROM node:20 as build
WORKDIR /app

# Копируем только package.json и package-lock.json (или yarn.lock), чтобы использовать кэш npm/yarn
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем весь код приложения
COPY . .

# Собираем проект с помощью Vite
RUN npm run build

# Stage 2: serve
FROM nginx:alpine

# Копируем собранные файлы в папку Nginx для сервировки
COPY --from=build /app/dist /usr/share/nginx/html
