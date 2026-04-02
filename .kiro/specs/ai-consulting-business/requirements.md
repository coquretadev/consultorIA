# Documento de Requisitos

## Introducción

Plataforma de herramientas para lanzar y operar un negocio de consultoría de integración de IA. El sistema permite a un consultor (desarrollador backend .NET con 4 años de experiencia) gestionar su catálogo de servicios, captar clientes, ejecutar proyectos de integración de IA en sistemas empresariales y hacer seguimiento financiero. La plataforma actúa como puente entre APIs de IA (OpenAI, Anthropic, Azure AI) y sistemas empresariales (ERPs, CRMs, bases de datos).

## Glosario

- **Plataforma**: Sistema web principal que agrupa todas las herramientas del negocio de consultoría
- **Consultor**: Usuario principal del sistema, el profesional que ofrece servicios de integración de IA
- **Cliente**: Empresa o persona que contrata los servicios de consultoría
- **Proyecto**: Unidad de trabajo contratada por un cliente, asociada a un servicio del catálogo
- **Catálogo_Servicios**: Conjunto de los 5 servicios ofrecidos: Chatbot RAG, Automatización documental, Copiloto interno, Integración APIs IA, Auditoría IA
- **Portal_Público**: Sitio web orientado a clientes potenciales que muestra servicios, casos de éxito y formulario de contacto
- **Panel_Consultor**: Dashboard privado donde el consultor gestiona proyectos, clientes y finanzas
- **Pipeline_Ventas**: Flujo de captación de clientes desde contacto inicial hasta cierre de contrato
- **Sector_Objetivo**: Segmento de mercado al que se dirigen los servicios: PYMEs .NET, SaaS B2B, Despachos legales, Agencias marketing
- **Plantilla_Proyecto**: Configuración predefinida con estructura, entregables y estimaciones para cada tipo de servicio
- **Roadmap_Formación**: Plan de aprendizaje técnico de 12 semanas que cubre LLMs, RAG, Function Calling, Agentes y Automatización documental
- **Proyección_Financiera**: Modelo de ingresos y gastos proyectado a 12 meses
- **Sistema_Idiomas**: Mecanismo de internacionalización del Portal_Público que permite mostrar contenido en múltiples idiomas
- **Analytics_Portal**: Sistema de seguimiento de métricas de visitas, tráfico y comportamiento de visitantes en el Portal_Público
- **SEO_Portal**: Conjunto de optimizaciones técnicas (meta tags, sitemap, Open Graph) para posicionamiento en buscadores
- **Widget_Calendario**: Componente embebido que permite a visitantes agendar llamadas de descubrimiento con el Consultor
- **Webhook_Notificación**: Mecanismo de notificación instantánea a servicios externos (Slack, Telegram) ante eventos del sistema
- **Rate_Limiter**: Mecanismo de protección que limita la frecuencia de peticiones al endpoint público de contacto
- **Estrategia_Migraciones**: Procedimiento definido para aplicar migraciones de EF Core en el entorno de producción de forma segura y controlada
- **Script_SQL_Migración**: Archivo SQL generado a partir de las migraciones de EF Core que permite aplicar cambios de esquema de forma manual y auditable
- **Documentación_Despliegue**: Documento operativo que describe la infraestructura, configuración y procedimientos necesarios para llevar la Plataforma a producción
- **Reverse_Proxy**: Servidor intermedio (nginx, Caddy) que gestiona HTTPS, compresión y reenvío de peticiones al backend de la Plataforma
- **Backup_PostgreSQL**: Procedimiento de respaldo periódico de la base de datos PostgreSQL para garantizar la recuperación ante fallos

## Requisitos

### Requisito 1: Portal Público de Servicios

**Historia de Usuario:** Como consultor, quiero un portal web público que presente mis servicios de integración de IA, para que clientes potenciales conozcan mi oferta y puedan contactarme.

#### Criterios de Aceptación

1. THE Portal_Público SHALL mostrar el Catálogo_Servicios con nombre, descripción, beneficios y rango de precio para cada uno de los 5 servicios
2. WHEN un visitante envía el formulario de contacto con nombre, email, empresa y mensaje, THE Portal_Público SHALL almacenar la solicitud, enviar una notificación por email al Consultor y enviar una Webhook_Notificación instantánea al canal configurado (Slack o Telegram)
3. THE Portal_Público SHALL mostrar una sección de casos de éxito con descripción del problema, solución aplicada y resultados obtenidos
4. THE Portal_Público SHALL mostrar los Sectores_Objetivo con una descripción de cómo la integración de IA beneficia a cada sector
5. WHEN un visitante accede al Portal_Público desde un dispositivo móvil, THE Portal_Público SHALL renderizar el contenido de forma adaptativa (responsive)
6. IF el formulario de contacto se envía con campos obligatorios vacíos, THEN THE Portal_Público SHALL mostrar mensajes de validación específicos por cada campo faltante
7. THE Rate_Limiter SHALL limitar las peticiones al endpoint de contacto del Portal_Público a un máximo de 5 envíos por dirección IP en un periodo de 15 minutos
8. IF una dirección IP supera el límite de envíos permitidos, THEN THE Rate_Limiter SHALL rechazar la petición con un mensaje indicando que se ha excedido el límite y el tiempo de espera restante

### Requisito 2: Panel de Gestión del Consultor

**Historia de Usuario:** Como consultor, quiero un panel privado donde gestionar mis proyectos, clientes y actividad comercial, para tener visibilidad completa de mi negocio.

#### Criterios de Aceptación

1. WHEN el Consultor inicia sesión con credenciales válidas, THE Panel_Consultor SHALL mostrar un dashboard con resumen de proyectos activos, ingresos del mes y tareas pendientes
2. THE Panel_Consultor SHALL permitir al Consultor crear, editar y archivar fichas de Cliente con nombre, empresa, sector, email, teléfono y notas
3. WHEN el Consultor crea un nuevo Proyecto, THE Panel_Consultor SHALL asociar el Proyecto a un Cliente y a un servicio del Catálogo_Servicios
4. THE Panel_Consultor SHALL mostrar una lista de todos los Proyectos con su estado (propuesta, en curso, completado, cancelado), cliente asociado y fechas
5. IF el Consultor intenta acceder al Panel_Consultor sin autenticación válida, THEN THE Panel_Consultor SHALL redirigir al formulario de inicio de sesión
6. WHEN el Consultor actualiza el estado de un Proyecto, THE Panel_Consultor SHALL registrar la fecha del cambio y mantener un historial de estados

### Requisito 3: Pipeline de Ventas y Captación de Clientes

**Historia de Usuario:** Como consultor, quiero gestionar mi pipeline de ventas en 3 fases, para hacer seguimiento de cada oportunidad comercial desde el contacto inicial hasta el cierre.

#### Criterios de Aceptación

1. THE Pipeline_Ventas SHALL organizar las oportunidades en las fases: Contacto inicial, Propuesta enviada, Negociación, Cerrado ganado, Cerrado perdido
2. WHEN el Consultor mueve una oportunidad entre fases, THE Pipeline_Ventas SHALL registrar la fecha de transición y calcular el tiempo en cada fase
3. WHEN se recibe una solicitud desde el formulario del Portal_Público, THE Pipeline_Ventas SHALL crear automáticamente una oportunidad en la fase "Contacto inicial"
4. THE Pipeline_Ventas SHALL mostrar una vista tipo kanban con las oportunidades agrupadas por fase
5. WHEN el Consultor marca una oportunidad como "Cerrado ganado", THE Pipeline_Ventas SHALL solicitar la creación de un Proyecto asociado
6. THE Pipeline_Ventas SHALL permitir al Consultor registrar el valor estimado en euros de cada oportunidad
7. IF una oportunidad permanece en la misma fase más de 14 días, THEN THE Pipeline_Ventas SHALL mostrar una alerta visual al Consultor

### Requisito 4: Gestión de Proyectos con Plantillas

**Historia de Usuario:** Como consultor, quiero iniciar proyectos a partir de plantillas predefinidas para cada tipo de servicio, para estandarizar entregables y reducir tiempo de arranque.

#### Criterios de Aceptación

1. THE Plataforma SHALL incluir una Plantilla_Proyecto para cada uno de los 5 servicios del Catálogo_Servicios
2. WHEN el Consultor crea un Proyecto desde una Plantilla_Proyecto, THE Plataforma SHALL generar automáticamente la lista de entregables, hitos y estimación de horas
3. THE Plataforma SHALL permitir al Consultor personalizar los entregables y hitos generados por la plantilla antes de confirmar el Proyecto
4. WHEN el Consultor marca un entregable como completado, THE Plataforma SHALL actualizar el porcentaje de avance del Proyecto
5. THE Plataforma SHALL permitir al Consultor registrar horas dedicadas a cada entregable de un Proyecto
6. IF el Consultor intenta crear un Proyecto sin asociar un Cliente, THEN THE Plataforma SHALL mostrar un mensaje indicando que el Cliente es obligatorio

### Requisito 5: Seguimiento Financiero

**Historia de Usuario:** Como consultor, quiero hacer seguimiento de ingresos, gastos y proyecciones financieras, para controlar la rentabilidad de mi negocio a 12 meses.

#### Criterios de Aceptación

1. THE Panel_Consultor SHALL mostrar un resumen financiero mensual con ingresos facturados, gastos registrados y beneficio neto
2. WHEN el Consultor registra una factura asociada a un Proyecto, THE Panel_Consultor SHALL sumar el importe a los ingresos del mes correspondiente
3. THE Panel_Consultor SHALL permitir al Consultor registrar gastos con categoría (herramientas, formación, marketing, infraestructura, otros), importe y fecha
4. THE Panel_Consultor SHALL mostrar una Proyección_Financiera a 12 meses basada en los ingresos y gastos actuales
5. WHEN el Consultor consulta la Proyección_Financiera, THE Panel_Consultor SHALL mostrar un gráfico con la evolución mensual de ingresos, gastos y beneficio neto
6. IF los gastos de un mes superan los ingresos de ese mes, THEN THE Panel_Consultor SHALL mostrar una alerta indicando resultado negativo

### Requisito 6: Checklist de Formación

**Historia de Usuario:** Como consultor, quiero un checklist sencillo de mi plan de formación técnica, para marcar los temas completados y ver mi progreso sin complejidad innecesaria.

#### Criterios de Aceptación

1. THE Plataforma SHALL mostrar el Roadmap_Formación como un checklist plano con los temas: LLMs, RAG, Function Calling, Agentes y Automatización documental
2. WHEN el Consultor marca un tema como completado en el checklist, THE Plataforma SHALL actualizar el porcentaje de avance global del Roadmap_Formación
3. THE Plataforma SHALL mostrar una barra de progreso visual con el avance del Roadmap_Formación en el dashboard del Panel_Consultor

### Requisito 7: Catálogo de Servicios Configurable

**Historia de Usuario:** Como consultor, quiero gestionar mi catálogo de servicios desde el panel, para poder actualizar descripciones, precios y disponibilidad sin modificar código.

#### Criterios de Aceptación

1. THE Panel_Consultor SHALL permitir al Consultor crear, editar y desactivar servicios del Catálogo_Servicios
2. WHEN el Consultor edita un servicio, THE Panel_Consultor SHALL solicitar nombre, descripción, beneficios, rango de precio, tiempo estimado de entrega y sector objetivo
3. WHEN el Consultor desactiva un servicio, THE Portal_Público SHALL dejar de mostrar ese servicio a los visitantes
4. THE Panel_Consultor SHALL permitir al Consultor reordenar los servicios del Catálogo_Servicios para controlar el orden de visualización en el Portal_Público
5. IF el Consultor intenta eliminar un servicio que tiene Proyectos asociados, THEN THE Panel_Consultor SHALL mostrar un aviso y solicitar confirmación antes de proceder


### Requisito 8: Multi-idioma del Portal Público

**Historia de Usuario:** Como consultor, quiero que mi portal público esté disponible en español e inglés, para captar clientes internacionales desde España.

#### Criterios de Aceptación

1. THE Sistema_Idiomas SHALL soportar al menos dos idiomas: español e inglés en todo el contenido del Portal_Público
2. THE Portal_Público SHALL mostrar un selector de idioma accesible desde cualquier página del portal
3. WHEN un visitante selecciona un idioma en el selector, THE Portal_Público SHALL mostrar todo el contenido (servicios, casos de éxito, formulario de contacto, textos de interfaz) en el idioma seleccionado
4. THE Portal_Público SHALL detectar el idioma preferido del navegador del visitante y mostrar el contenido en ese idioma si está disponible
5. WHEN el Consultor edita un servicio del Catálogo_Servicios, THE Panel_Consultor SHALL solicitar el contenido en cada idioma soportado por el Sistema_Idiomas

### Requisito 9: Analytics del Portal Público

**Historia de Usuario:** Como consultor, quiero ver métricas de visitas y comportamiento en mi portal público, para optimizar la captación de clientes y entender qué servicios generan más interés.

#### Criterios de Aceptación

1. THE Analytics_Portal SHALL registrar cada visita al Portal_Público con fecha, página visitada, origen del tráfico y dispositivo utilizado
2. THE Panel_Consultor SHALL mostrar un dashboard de métricas con número de visitas totales, visitantes únicos, páginas más vistas y origen del tráfico en un periodo seleccionable
3. WHEN un visitante consulta la página de un servicio del Catálogo_Servicios, THE Analytics_Portal SHALL registrar el servicio consultado para identificar los servicios con mayor interés
4. THE Panel_Consultor SHALL mostrar un ranking de los servicios más consultados por los visitantes del Portal_Público
5. WHEN el Consultor selecciona un periodo de tiempo en el dashboard de métricas, THE Panel_Consultor SHALL filtrar todas las métricas al rango de fechas seleccionado

### Requisito 10: SEO del Portal Público

**Historia de Usuario:** Como consultor, quiero que mi portal público esté optimizado para buscadores y redes sociales, para que clientes potenciales me encuentren fácilmente en Google y puedan compartir mis servicios.

#### Criterios de Aceptación

1. THE SEO_Portal SHALL generar meta tags (title, description, keywords) específicos para cada página del Portal_Público
2. THE SEO_Portal SHALL generar un archivo sitemap.xml actualizado con todas las páginas públicas del Portal_Público
3. THE SEO_Portal SHALL incluir Open Graph tags (og:title, og:description, og:image, og:url) en cada página del Portal_Público para optimizar la previsualización al compartir en redes sociales
4. WHEN el Consultor edita un servicio del Catálogo_Servicios, THE Panel_Consultor SHALL permitir personalizar el meta title y meta description de la página de ese servicio
5. THE SEO_Portal SHALL generar URLs amigables (slugs legibles) para cada servicio y caso de éxito del Portal_Público

### Requisito 11: Integración con Calendario para Agendar Llamadas

**Historia de Usuario:** Como consultor, quiero que los visitantes de mi portal puedan agendar llamadas de descubrimiento directamente, para reducir la fricción en el proceso de captación y acelerar el primer contacto.

#### Criterios de Aceptación

1. THE Portal_Público SHALL mostrar un Widget_Calendario embebido en la página de contacto que permita a los visitantes seleccionar fecha y hora para una llamada de descubrimiento
2. WHEN un visitante selecciona un horario disponible en el Widget_Calendario, THE Portal_Público SHALL solicitar nombre, email y empresa del visitante antes de confirmar la reserva
3. WHEN un visitante confirma una reserva en el Widget_Calendario, THE Plataforma SHALL enviar un email de confirmación al visitante y una Webhook_Notificación al Consultor
4. THE Panel_Consultor SHALL permitir al Consultor configurar su disponibilidad horaria semanal para las llamadas de descubrimiento
5. WHEN un visitante agenda una llamada desde el Widget_Calendario, THE Pipeline_Ventas SHALL crear automáticamente una oportunidad en la fase "Contacto inicial" asociada a los datos del visitante
6. IF un visitante intenta reservar un horario que ya no está disponible, THEN THE Widget_Calendario SHALL mostrar un mensaje indicando que el horario fue ocupado y ofrecer horarios alternativos


### Requisito 12: Estrategia de Migraciones de Base de Datos en Producción

**Historia de Usuario:** Como consultor, quiero una estrategia clara para aplicar migraciones de EF Core en producción, para que los cambios de esquema de base de datos se apliquen de forma segura, auditable y sin riesgo de pérdida de datos al desplegar nuevas versiones.

#### Criterios de Aceptación

1. THE Plataforma SHALL generar un Script_SQL_Migración a partir de cada migración de EF Core antes de aplicar cambios en el entorno de producción
2. THE Estrategia_Migraciones SHALL prohibir la ejecución de migraciones automáticas de EF Core al arrancar la aplicación en el entorno de producción
3. WHEN el Consultor prepara un despliegue con cambios de esquema, THE Estrategia_Migraciones SHALL requerir la revisión manual del Script_SQL_Migración antes de su aplicación en producción
4. THE Estrategia_Migraciones SHALL incluir un procedimiento documentado para revertir una migración aplicada en producción en caso de error
5. IF una migración de EF Core incluye operaciones destructivas (eliminación de tablas o columnas con datos), THEN THE Estrategia_Migraciones SHALL requerir un Backup_PostgreSQL previo a la aplicación del Script_SQL_Migración
6. THE Plataforma SHALL registrar en un log la fecha, nombre y resultado de cada migración aplicada en producción

### Requisito 13: Documentación de Despliegue a Producción

**Historia de Usuario:** Como consultor, quiero documentación completa de despliegue a producción, para poder llevar la plataforma a un entorno real de forma reproducible, segura y sin depender de conocimiento no documentado.

#### Criterios de Aceptación

1. THE Documentación_Despliegue SHALL describir la infraestructura mínima necesaria para ejecutar la Plataforma en producción, incluyendo servidor, sistema operativo, versión de .NET y versión de PostgreSQL
2. THE Documentación_Despliegue SHALL listar todas las variables de entorno y configuraciones requeridas en producción, incluyendo cadena de conexión a PostgreSQL, clave JWT, issuer, audience y URLs de webhook
3. THE Documentación_Despliegue SHALL incluir la configuración de un Reverse_Proxy con terminación HTTPS para exponer la Plataforma de forma segura
4. THE Documentación_Despliegue SHALL incluir un procedimiento de Backup_PostgreSQL automatizado con frecuencia mínima diaria y verificación de integridad del respaldo
5. WHEN el Consultor despliega una nueva versión de la Plataforma, THE Documentación_Despliegue SHALL proporcionar un procedimiento paso a paso que incluya parada del servicio, aplicación de migraciones, despliegue del binario y verificación de salud del sistema
6. THE Documentación_Despliegue SHALL incluir la configuración de CORS en producción restringida al dominio real de la Plataforma en lugar de permitir cualquier origen
7. IF el servicio de la Plataforma deja de responder en producción, THEN THE Documentación_Despliegue SHALL describir un procedimiento de diagnóstico que incluya verificación de logs, estado de PostgreSQL y conectividad del Reverse_Proxy
