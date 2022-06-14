### STAGE 1: Build ###
FROM node:16.10-alpine AS build
WORKDIR /usr/src/app
COPY ChatBot/webapp/package.json ChatBot/webapp/package-lock.json ./
RUN npm install
RUN npm install -g @angular/cli
COPY . .
WORKDIR "/usr/src/app/ChatBot/webapp"
RUN npm run build

### STAGE 2: Run ###
FROM nginx:1.17.1-alpine
COPY ChatBot/webapp/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/ChatBot/webapp/dist/webapp /usr/share/nginx/html
