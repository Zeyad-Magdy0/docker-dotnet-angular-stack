# -----------------------------
# Build stage (Angular build)
# -----------------------------
FROM node:18 AS build
WORKDIR /app

# Copy frontend source / build output
COPY . .

# In a real Angular app, this would be:
# RUN npm install
# RUN ng build
#
# For now, we assume dist/ already exists


# -----------------------------
# Runtime stage (NGINX)
# -----------------------------

FROM nginx:alpine

# Remove default NGINX config
RUN rm /etc/nginx/conf.d/default.conf

# Copy custom NGINX config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built frontend files
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80