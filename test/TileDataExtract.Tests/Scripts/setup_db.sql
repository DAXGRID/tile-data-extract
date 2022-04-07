-- Enable PostGIS on database if not already enabled
CREATE EXTENSION IF NOT EXISTS postgis;

-- Enable UUID-OSSP
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE SCHEMA IF NOT EXISTS route_network;

--- Create route_network.route_node table
CREATE TABLE route_network.route_node (
	mrid uuid NOT NULL DEFAULT uuid_generate_v4(),
	coord public.geometry(point, 25832) NULL,
	marked_to_be_deleted bool NOT NULL,
	delete_me bool NOT NULL,
	work_task_mrid uuid NULL,
	user_name varchar(255) NULL,
	application_name varchar(255) NULL,
	application_info varchar NULL,
	lifecycle_deployment_state varchar(255) NULL,
	lifecycle_installation_date timestamptz NULL,
	lifecycle_removal_date timestamptz NULL,
	mapping_method varchar(255) NULL,
	mapping_vertical_accuracy varchar(255) NULL,
	mapping_horizontal_accuracy varchar(255) NULL,
	mapping_source_info text NULL,
	mapping_survey_date timestamptz NULL,
	safety_classification varchar(255) NULL,
	safety_remark text NULL,
	routenode_kind varchar(255) NULL,
	routenode_function varchar(255) NULL,
	naming_name varchar(255) NULL,
	naming_description varchar(2048) NULL,
	lifecycle_documentation_state varchar(50) NULL,
    PRIMARY KEY(mrid)
);

CREATE INDEX route_node_coord_idx
    ON route_network.route_node USING gist ("coord");

--- Insert test data into route_network.route_node table
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('06e660e2-8a6b-4f1b-bc7f-85f1aea8ca5f'::uuid, 'SRID=25832;POINT (552543.1451123824 6189458.576412785)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('4adfcc8a-c613-4885-9510-0b8aa1518c51'::uuid, 'SRID=25832;POINT (552610.3821794558 6189524.559056965)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('06b9549f-6437-43e6-a34b-22372567f64e'::uuid, 'SRID=25832;POINT (552577.4606094105 6189492.251695316)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, 'GDB_INTEGRATOR', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('c128d796-861d-433f-bc37-de93de0d22e3'::uuid, 'SRID=25832;POINT (552520.3146157267 6189550.901937721)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('7293d541-eb38-4a25-9910-016ea13cb4ec'::uuid, 'SRID=25832;POINT (552561.4316413166 6189476.521775135)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, 'GDB_INTEGRATOR', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('fa0759cb-f42a-41cb-b9e9-e97d84402089'::uuid, 'SRID=25832;POINT (552606.4934684869 6189441.516261438)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('50279e46-b8fb-4538-b830-8ebce4546f66'::uuid, 'SRID=25832;POINT (552579.4023252585 6189462.561555091)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, 'GDB_INTEGRATOR', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('0415770b-27e6-421c-b6bb-7a40dc2165f6'::uuid, 'SRID=25832;POINT (552545.2776313006 6189423.954340934)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('619d64a1-35fc-439f-adc9-2df6c0312d39'::uuid, 'SRID=25832;POINT (552471.8784904125 6189445.274261136)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO route_network.route_node (mrid, coord, marked_to_be_deleted, delete_me, work_task_mrid, user_name, application_name, application_info, lifecycle_deployment_state, lifecycle_installation_date, lifecycle_removal_date, mapping_method, mapping_vertical_accuracy, mapping_horizontal_accuracy, mapping_source_info, mapping_survey_date, safety_classification, safety_remark, routenode_kind, routenode_function, naming_name, naming_description, lifecycle_documentation_state) VALUES('6923e700-3c40-4e07-aad0-ab0dc122d97a'::uuid, 'SRID=25832;POINT (552471.8784904125 6189505.389286504)'::public.geometry, false, false, '00000000-0000-0000-0000-000000000000'::uuid, '', 'GDB_INTEGRATOR', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
