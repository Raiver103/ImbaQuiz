# Используем официальный Node.js образ для сборки
FROM node:16 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем package.json и устанавливаем зависимости
COPY imbaquiz.front/package.json imbaquiz.front/package-lock.json ./
RUN npm install

# Копируем исходный код фронтенда
COPY imbaquiz.front/ ./

# Строим проект
RUN npm run build

# Используем Nginx для сервировки фронтенда
FROM nginx:alpine

# Копируем билд проекта в Nginx
COPY --from=build /app/build /usr/share/nginx/html

# Открываем порт 80 для доступа к фронтенду
EXPOSE 80

# Запускаем Nginx
CMD ["nginx", "-g", "daemon off;"]
