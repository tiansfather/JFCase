/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 80011
Source Host           : localhost:3306
Source Database       : jfdb

Target Server Type    : MYSQL
Target Server Version : 80011
File Encoding         : 65001

Date: 2019-05-30 12:49:07
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for auditlog
-- ----------------------------
DROP TABLE IF EXISTS `auditlog`;
CREATE TABLE `auditlog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TenantId` int(11) DEFAULT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `ServiceName` longtext,
  `MethodName` longtext,
  `Parameters` longtext,
  `ExecutionTime` datetime(6) NOT NULL,
  `ExecutionDuration` int(11) NOT NULL,
  `ClientIpAddress` longtext,
  `ClientName` longtext,
  `BrowserInfo` longtext,
  `Exception` longtext,
  `ImpersonatorUserId` bigint(20) DEFAULT NULL,
  `ImpersonatorTenantId` int(11) DEFAULT NULL,
  `CustomData` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AuditLog_TenantId_ExecutionDuration` (`TenantId`,`ExecutionDuration`),
  KEY `IX_AuditLog_TenantId_ExecutionTime` (`TenantId`,`ExecutionTime`),
  KEY `IX_AuditLog_TenantId_UserId` (`TenantId`,`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=413 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of auditlog
-- ----------------------------
INSERT INTO `auditlog` VALUES ('1', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-23 19:54:44.050611', '141', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('2', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 19:54:44.884628', '821', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('3', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 19:54:46.093659', '86', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('4', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 19:54:48.946307', '12', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('5', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 19:54:52.936695', '149', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('6', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 19:54:52.935718', '152', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('7', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-23 19:55:26.712817', '0', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('8', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:55:27.877781', '37', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('9', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:55:30.138379', '335', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('10', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:55:31.809170', '4', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('11', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":null,\"name\":\"审判组织\",\"displayName\":\"审判组织\",\"briefCode\":\"a\",\"sort\":1,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:55:49.630295', '173', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('12', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:55:49.863679', '14', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('13', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":1}', '2019-05-23 19:55:51.642862', '2', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('14', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:55:52.335200', '5', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('15', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":1,\"name\":\"上海\",\"displayName\":\"上海\",\"briefCode\":\"shanghai\",\"sort\":1,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:56:00.472375', '53', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('16', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:56:00.612991', '4', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('17', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":1}', '2019-05-23 19:56:06.897745', '1', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('18', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:56:07.575436', '4', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('19', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":1,\"name\":\"北京\",\"displayName\":\"北京\",\"briefCode\":\"beijing\",\"sort\":2,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:56:41.943353', '95', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('20', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:56:42.093734', '6', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('21', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":2}', '2019-05-23 19:56:43.650275', '0', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('22', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:56:44.306483', '5', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('23', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":2,\"name\":\"上海市松江区人民法院\",\"displayName\":\"上海市松江区人民法院\",\"briefCode\":\"court1\",\"sort\":1,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:57:06.652709', '60', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('24', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:57:06.781607', '8', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('25', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":2}', '2019-05-23 19:57:16.180420', '1', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('26', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:57:16.911818', '6', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('27', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":2,\"name\":\"上海市浦东新区人民法院\",\"displayName\":\"上海市浦东新区人民法院\",\"briefCode\":\"a2\",\"sort\":2,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:57:23.541277', '71', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('28', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:57:23.684822', '5', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('29', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":4}', '2019-05-23 19:57:26.463941', '27', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('30', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":4}', '2019-05-23 19:57:27.414076', '4', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('31', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:57:27.484384', '5', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('32', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":5}', '2019-05-23 19:57:31.859104', '3', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('33', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":5}', '2019-05-23 19:57:32.470393', '0', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('34', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-23 19:57:32.540701', '6', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('35', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":5,\"parentId\":2,\"name\":\"上海市浦东新区人民法院\",\"displayName\":\"上海市浦东新区人民法院\",\"briefCode\":\"court2\",\"sort\":2,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-23 19:57:36.143009', '58', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('36', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-23 19:57:36.258236', '5', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('37', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 19:57:39.203360', '9', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('38', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 19:57:39.928900', '4', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('39', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 19:57:39.918158', '17', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('40', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Import', '{}', '2019-05-23 19:59:46.881712', '0', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('41', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-23 20:03:21.676558', '10', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('42', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 20:03:21.725383', '89', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('43', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 20:03:21.845492', '92', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('44', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 20:03:50.886602', '8', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('45', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 20:03:54.839474', '13', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('46', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 20:03:54.853145', '4', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('47', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-23 20:05:25.943995', '9', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('48', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 20:05:25.986961', '100', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('49', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-23 20:05:26.124647', '101', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('50', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 20:05:32.650597', '12', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('51', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 20:05:37.069259', '5', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('52', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 20:05:37.065353', '12', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('53', '1', '2', 'Master.Web.Controllers.FileController', 'CommonUpload', '{}', '2019-05-23 20:05:43.352060', '1', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('54', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 20:07:45.980930', '8', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('55', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 20:07:47.704453', '14', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('56', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 20:07:47.691758', '32', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('57', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-23 20:13:36.007355', '22', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('58', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-23 20:13:37.308053', '10', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('59', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-23 20:13:37.359808', '4', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('60', '1', '2', 'Master.Web.Controllers.HomeController', 'Index', '{}', '2019-05-24 20:26:29.616050', '307', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('61', '1', '2', 'Master.Web.Controllers.ErrorController', 'Index', '{}', '2019-05-24 20:26:30.618915', '3', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('62', '1', '2', 'Master.Web.Controllers.AccountController', 'Login', '{}', '2019-05-24 20:27:00.218583', '64', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('63', '1', '2', 'Master.Controllers.TokenAuthController', 'GetExternalAuthenticationProviders', '{}', '2019-05-24 20:27:04.311095', '9', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('64', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-26 11:16:20.045327', '61', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('65', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 11:16:20.782660', '850', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('66', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 11:16:22.099117', '80', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('67', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-26 11:16:26.562179', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('68', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:16:28.646244', '110', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('69', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:16:35.634793', '11', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('70', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:16:36.941484', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('71', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-26 11:17:39.475135', '14', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('72', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 11:17:40.189030', '94', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('73', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 11:17:40.502518', '87', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('74', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-26 11:17:43.860069', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('75', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:17:45.518336', '8', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('76', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-26 11:17:47.927608', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('77', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:17:48.256722', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('78', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":4}', '2019-05-26 11:17:51.468760', '2', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('79', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:17:52.589897', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('80', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":1}', '2019-05-26 11:17:57.885998', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('81', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:17:58.677044', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('82', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":1,\"name\":\"长沙\",\"displayName\":\"长沙\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:18:04.495627', '206', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('83', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:18:04.758333', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('84', '1', '2', 'Master.Web.Controllers.CaseController', 'Index', '{}', '2019-05-26 11:19:05.207919', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('85', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseInitial\"}', '2019-05-26 11:19:06.931618', '4', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('86', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-26 11:19:07.205066', '357', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('87', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"city\\\",\\\"anYou\\\",\\\"title\\\",\\\"processor\\\",\\\"publisDate\\\",\\\"praiseNumber\\\",\\\"beatNumber\\\",\\\"caseStatus\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-26 11:19:07.684577', '141', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('88', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:20:50.856507', '1', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('89', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:20:51.488368', '9', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('90', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":null,\"name\":\"专题\",\"displayName\":\"专题\",\"briefCode\":null,\"sort\":1,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:20:57.412423', '34', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('91', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:20:57.512036', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('92', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":7}', '2019-05-26 11:20:59.633212', '1', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('93', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:21:00.261165', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('94', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":7,\"name\":\"专题一\",\"displayName\":\"专题一\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:21:04.091391', '53', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('95', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:21:04.207606', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('96', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:21:18.611479', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('97', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:21:19.286310', '5', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('98', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":null,\"name\":\"纠纷属性\",\"displayName\":\"纠纷属性\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:21:24.139035', '53', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('99', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:21:24.253298', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('100', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":9}', '2019-05-26 11:22:57.882869', '1', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('101', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:22:58.509847', '10', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('102', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":9,\"name\":\"房屋买卖纠纷（二手）\",\"displayName\":\"房屋买卖纠纷（二手）\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:22:59.747199', '34', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('103', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:22:59.840952', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('104', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":9}', '2019-05-26 11:23:09.478041', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('105', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:23:10.156778', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('106', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":9,\"name\":\"房屋买卖纠纷（期房）\",\"displayName\":\"房屋买卖纠纷（期房）\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:23:11.356043', '39', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('107', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:23:11.466399', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('108', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":9}', '2019-05-26 11:23:37.873663', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('109', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:23:38.564119', '10', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('110', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":9,\"name\":\"商品房销售合同纠纷\",\"displayName\":\"商品房销售合同纠纷\",\"briefCode\":null,\"sort\":3,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:23:40.835691', '40', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('111', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:23:40.931397', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('112', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":10}', '2019-05-26 11:25:25.176611', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('113', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:25:25.894412', '13', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('114', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":10,\"name\":\"定金纠纷\",\"displayName\":\"定金纠纷\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:25:30.609437', '59', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('115', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:25:30.716863', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('116', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":13}', '2019-05-26 11:25:32.937651', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('117', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:25:33.551933', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('118', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":13,\"name\":\"纠纷原因\",\"displayName\":\"纠纷原因\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:26:04.307997', '46', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('119', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:26:04.428118', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('120', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":13}', '2019-05-26 11:26:07.137207', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('121', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:26:07.812037', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('122', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":13,\"name\":\"判决结果\",\"displayName\":\"判决结果\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:26:12.743867', '46', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('123', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:26:12.862036', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('124', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-26 11:26:43.229413', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('125', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:26:43.901314', '11', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('126', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"无权代理\",\"displayName\":\"无权代理\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:26:50.167179', '41', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('127', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:26:50.271676', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('128', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-26 11:26:51.745365', '0', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('129', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:26:52.364529', '8', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('130', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"无权处分\",\"displayName\":\"无权处分\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:26:56.165457', '29', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('131', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:26:56.250421', '6', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('132', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-26 11:27:04.157951', '1', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('133', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:27:04.876729', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('134', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"双倍退还\",\"displayName\":\"双倍退还\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:27:08.765550', '40', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('135', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:27:08.869069', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('136', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-26 11:27:10.356431', '1', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('137', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 11:27:10.975596', '8', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('138', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"没收定金\",\"displayName\":\"没收定金\",\"briefCode\":null,\"sort\":0,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-26 11:27:15.119309', '42', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('139', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 11:27:15.225759', '7', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('140', '1', '2', 'Master.Web.Controllers.HomeController', 'Index', '{}', '2019-05-26 22:47:43.891234', '263', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('141', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-26 22:48:06.229983', '10', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('142', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 22:48:06.923369', '1072', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('143', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-26 22:48:08.384362', '94', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('144', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-26 22:48:21.265716', '0', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('145', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-26 22:48:23.233565', '118', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('146', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":null}', '2019-05-26 22:48:32.976127', '7', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('147', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-26 22:48:34.087498', '9', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('148', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 06:51:43.526305', '77', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('149', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 06:51:44.270474', '945', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('150', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 06:51:45.718772', '81', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('151', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-27 06:51:50.908424', '14', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('152', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-27 06:51:55.090225', '261', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('153', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-27 06:51:55.090225', '261', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('154', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-27 06:52:15.994348', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('155', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-27 06:52:17.575464', '6', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('156', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-27 06:52:17.583277', '6', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('157', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Import', '{}', '2019-05-27 06:52:52.489890', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('158', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 06:52:56.793767', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('159', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 06:52:58.896386', '4', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('160', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 06:52:59.257728', '342', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('161', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 06:52:59.807554', '138', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('162', '1', '2', 'Master.Web.Controllers.CaseController', 'Index', '{}', '2019-05-27 06:53:01.883806', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('163', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseInitial\"}', '2019-05-27 06:53:03.024475', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('164', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 06:53:03.278391', '259', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('165', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 06:53:03.629967', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('166', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"city\\\",\\\"anYou\\\",\\\"title\\\",\\\"processor\\\",\\\"publisDate\\\",\\\"praiseNumber\\\",\\\"beatNumber\\\",\\\"caseStatus\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 06:53:03.709071', '82', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('167', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 06:53:04.606567', '24', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('168', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 06:54:58.089440', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('169', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 06:54:58.687119', '13', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('170', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":16}', '2019-05-27 06:55:06.072742', '27', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('171', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":16}', '2019-05-27 06:55:07.444029', '4', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('172', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 06:55:07.510444', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('173', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":14}', '2019-05-27 06:55:18.761052', '3', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('174', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":14}', '2019-05-27 06:55:19.918441', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('175', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 06:55:20.076666', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('176', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":4}', '2019-05-27 06:55:31.916224', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('177', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 06:55:32.951526', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('178', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 17:19:33.099595', '91', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('179', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:19:33.908220', '923', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('180', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:19:35.211005', '90', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('181', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 17:19:55.966684', '15', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('182', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:19:56.037000', '87', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('183', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:19:56.155168', '104', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('184', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 17:20:08.585333', '19', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('185', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:20:08.742566', '91', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('186', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 17:20:08.864641', '107', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('187', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 17:40:05.002367', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('188', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 17:40:07.325699', '465', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('189', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 17:40:15.195142', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('190', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 17:40:15.535975', '11', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('191', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 17:40:17.674729', '12', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('192', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 17:40:18.009703', '13', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('193', '1', '2', 'Master.Web.Controllers.CaseController', 'Index', '{}', '2019-05-27 17:42:20.027083', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('194', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseInitial\"}', '2019-05-27 17:42:21.573041', '4', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('195', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 17:42:21.837700', '429', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('196', '1', '2', 'Master.Case.CaseInitialAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"city\\\",\\\"anYou\\\",\\\"title\\\",\\\"processor\\\",\\\"publisDate\\\",\\\"praiseNumber\\\",\\\"beatNumber\\\",\\\"caseStatus\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 17:42:22.355298', '157', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('197', '1', '2', 'Master.Web.Controllers.AccountController', 'GMLogin', '{}', '2019-05-27 19:36:03.901415', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('198', '1', '2', 'Master.Web.Controllers.VerifyCodeController', 'NumberVerifyCode', '{}', '2019-05-27 19:36:05.052826', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('199', '1', '2', 'Master.Controllers.TokenAuthController', 'GetExternalAuthenticationProviders', '{}', '2019-05-27 19:36:05.055756', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('200', '1', '2', 'Master.Controllers.TokenAuthController', 'Authenticate', '{\"model\":{\"userName\":\"admin\",\"password\":\"12345678\",\"tenancyName\":\"Default\",\"verifyCode\":\"5924\"}}', '2019-05-27 19:36:18.062115', '171', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('201', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 19:36:19.807299', '16', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('202', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 19:36:19.863942', '92', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('203', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 19:36:19.981134', '112', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('204', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 19:36:44.526998', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('205', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 19:36:45.360038', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('206', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:36:45.644229', '203', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('207', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:36:45.938185', '110', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('208', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 19:37:28.303093', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('209', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 19:37:28.912492', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('210', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:37:28.979877', '18', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('211', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:37:29.087303', '22', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('212', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-27 19:37:40.360197', '14', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('213', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-27 19:37:41.428597', '265', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('214', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-27 19:37:41.429574', '261', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('215', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 19:37:57.810086', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('216', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 19:37:58.591366', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('217', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:37:58.735902', '21', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('218', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:37:58.841375', '26', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('219', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 19:38:01.726252', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('220', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 19:38:02.153026', '16', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('221', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentId', '{\"id\":2}', '2019-05-27 19:38:39.653489', '9', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('222', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":1}', '2019-05-27 19:38:51.057247', '9', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('223', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 19:38:52.110999', '14', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('224', '1', '2', 'Master.Web.Controllers.FileController', 'Upload', '{}', '2019-05-27 19:46:49.726995', '30', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('225', '1', '2', 'Master.Case.CaseSourceAppService', 'Update', '{\"caseSourceDto\":{\"id\":null,\"sourceSN\":\"111\",\"anYouId\":10,\"cityId\":2,\"court1Id\":4,\"court2Id\":5,\"validDate\":\"2019-05-30T00:00:00\",\"trialPeople\":[{\"name\":\"a\",\"trialRole\":0}],\"lawyerFirms\":[{\"firmName\":\"qqqq\",\"lawyer\":\"qqq\"}],\"sourceFile\":\"/files/2019/05/27/6f9191a7-ace2-4b9b-b87f-2b3c86ee7a1f.pdf\"}}', '2019-05-27 19:47:02.480414', '245', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('226', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{}', '2019-05-27 19:47:03.002895', '10', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('227', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-27 19:47:03.629872', '38', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('228', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-27 19:47:03.625966', '42', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('229', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 19:47:05.988361', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('230', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 19:47:06.769641', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('231', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:47:06.837027', '525', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('232', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:47:07.450332', '21', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('233', '1', '2', 'Master.Web.Controllers.BusinessUserController', 'NewMiner', '{}', '2019-05-27 19:52:08.080039', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('234', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"NewMiner\"}', '2019-05-27 19:52:09.003903', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('235', '1', '2', 'Master.Users.NewMinerAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:52:09.295906', '602', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('236', '1', '2', 'Master.Users.NewMinerAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"avata\\\",\\\"nickName\\\",\\\"name\\\",\\\"workLocation\\\",\\\"email\\\",\\\"creationTime\\\",\\\"remarks\\\",\\\"creatorUserId\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 19:52:10.099648', '68', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('237', '1', '2', 'Master.Web.Controllers.SettingController', 'Index', '{}', '2019-05-27 20:02:22.342709', '29', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('238', '1', '2', 'Master.Web.Controllers.HomeController', 'Index', '{}', '2019-05-27 21:56:46.796434', '123', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('239', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 21:57:01.807192', '7', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('240', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 21:57:01.852111', '317', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('241', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 21:57:02.195839', '112', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('242', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-27 21:57:05.372394', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('243', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-27 21:57:06.694575', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('244', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 21:57:06.777577', '100', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('245', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-27 21:57:07.013890', '19', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', null, null, null, null);
INSERT INTO `auditlog` VALUES ('246', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 22:11:49.380267', '6', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('247', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 22:11:49.446669', '92', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('248', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 22:11:49.564825', '97', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('249', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 22:12:00.963510', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('250', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:12:01.452736', '32', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('251', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:13:40.845789', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('252', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:13:41.526409', '12', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('253', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"违反协议\",\"displayName\":\"违反协议\",\"briefCode\":null,\"sort\":3,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:13:52.877245', '152', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('254', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:13:53.085240', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('255', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":17}', '2019-05-27 22:13:59.247931', '24', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('256', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":17}', '2019-05-27 22:14:00.323058', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('257', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:14:00.406060', '11', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('258', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":16}', '2019-05-27 22:14:03.794515', '6', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('259', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":16}', '2019-05-27 22:14:04.579621', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('260', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:14:04.717308', '11', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('261', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-27 22:16:24.125377', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('262', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 22:16:24.163461', '93', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('263', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-27 22:16:24.282594', '83', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('264', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 22:16:28.379011', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('265', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:16:28.760823', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('266', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:16:51.993711', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('267', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:16:52.652848', '7', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('268', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"限购状态\",\"displayName\":\"限购状态\",\"briefCode\":null,\"sort\":4,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:17:03.229320', '34', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('269', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:17:03.322087', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('270', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:17:20.848309', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('271', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:17:21.568966', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('272', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"未尽事宜\",\"displayName\":\"未尽事宜\",\"briefCode\":null,\"sort\":5,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:17:33.321144', '31', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('273', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:17:33.405123', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('274', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:17:37.596261', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('275', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:17:38.528818', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('276', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"约定不明\",\"displayName\":\"约定不明\",\"briefCode\":null,\"sort\":6,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:17:47.716707', '34', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('277', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:17:47.812404', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('278', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:18:21.578797', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('279', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:18:22.270159', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('280', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"履行不能\",\"displayName\":\"履行不能\",\"briefCode\":null,\"sort\":6,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:18:30.229611', '29', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('281', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:18:30.328237', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('282', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:18:40.317832', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('283', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:18:40.986735', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('284', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"定金认定\",\"displayName\":\"定金认定\",\"briefCode\":null,\"sort\":10,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:18:51.066168', '40', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('285', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:18:51.154053', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('286', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:18:55.280742', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('287', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:18:56.232829', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('288', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"其他\",\"displayName\":\"其他\",\"briefCode\":null,\"sort\":20,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:19:03.997957', '49', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('289', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:19:04.104396', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('290', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":14}', '2019-05-27 22:19:52.980174', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('291', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:19:53.628570', '12', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('292', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":14,\"name\":\"定金返还\",\"displayName\":\"定金返还\",\"briefCode\":null,\"sort\":20,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:20:01.359520', '64', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('293', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:20:01.484512', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('294', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-27 22:20:10.748568', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('295', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:20:11.447742', '8', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('296', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"定金返还\",\"displayName\":\"定金返还\",\"briefCode\":null,\"sort\":3,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:20:19.961845', '35', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('297', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:20:20.074143', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('298', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-27 22:20:23.252650', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('299', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:20:23.880540', '11', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('300', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-27 22:21:04.486339', '1', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('301', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:21:05.153289', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('302', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"赔偿实际损失\",\"displayName\":\"赔偿实际损失\",\"briefCode\":null,\"sort\":20,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:21:20.025384', '42', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('303', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:21:20.115222', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('304', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-27 22:21:24.068094', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('305', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:21:24.695983', '9', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('306', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"赔偿违约金\",\"displayName\":\"赔偿违约金\",\"briefCode\":null,\"sort\":30,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:21:35.829060', '40', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('307', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:21:36.000924', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('308', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Add', '{\"treeKey\":\"BaseType\",\"parentId\":15}', '2019-05-27 22:21:40.477200', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('309', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-27 22:21:41.099230', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('310', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'Submit', '{\"baseTreeDto\":{\"id\":0,\"parentId\":15,\"name\":\"其他\",\"displayName\":\"其他\",\"briefCode\":null,\"sort\":40,\"remarks\":null,\"discriminator\":\"BaseType\"}}', '2019-05-27 22:21:51.552663', '35', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('311', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:21:51.642501', '10', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('312', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-27 22:23:30.556092', '0', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('313', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-27 22:23:31.081449', '29', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('314', '1', '2', 'Master.Web.Controllers.AccountController', 'GMLogin', '{}', '2019-05-28 08:45:57.515147', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('315', '1', '2', 'Master.Web.Controllers.AccountController', 'GMLogin', '{}', '2019-05-28 08:45:58.654956', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('316', '1', '2', 'Master.Controllers.TokenAuthController', 'GetExternalAuthenticationProviders', '{}', '2019-05-28 08:46:00.688445', '8', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('317', '1', '2', 'Master.Web.Controllers.VerifyCodeController', 'NumberVerifyCode', '{}', '2019-05-28 08:46:00.534126', '230', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('318', '1', '2', 'Master.Web.Controllers.AccountController', 'GMLogin', '{}', '2019-05-28 08:46:45.527765', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('319', '1', '2', 'Master.Controllers.TokenAuthController', 'GetExternalAuthenticationProviders', '{}', '2019-05-28 08:46:46.098158', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('320', '1', '2', 'Master.Web.Controllers.VerifyCodeController', 'NumberVerifyCode', '{}', '2019-05-28 08:46:46.097181', '3', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('321', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-28 08:46:49.404288', '47', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('322', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 08:46:50.192484', '214', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('323', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 08:46:50.788271', '84', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('324', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-28 08:46:53.979150', '0', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('325', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-28 08:46:56.005803', '118', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('326', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-28 16:34:13.041219', '10', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('327', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 16:34:13.161341', '331', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('328', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 16:34:13.529519', '93', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('329', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-28 16:34:22.565999', '0', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('330', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-28 16:34:23.191023', '52', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('331', '1', '2', 'Master.Web.Controllers.BaseTreeController', 'Edit', '{\"id\":17}', '2019-05-28 17:25:58.445639', '49', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('332', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetBaseTree', '{\"id\":17}', '2019-05-28 17:26:00.747249', '4', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('333', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\"}', '2019-05-28 17:26:00.913254', '20', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('334', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-28 22:29:47.813597', '6', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('335', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 22:29:47.904421', '242', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('336', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-28 22:29:48.176893', '89', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('337', '1', '2', 'Master.Web.Controllers.CaseController', 'Types', '{}', '2019-05-28 22:29:56.644015', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('338', '1', '2', 'Master.BaseTrees.BaseTreeAppService', 'GetTreeJson', '{\"discriminator\":\"BaseType\",\"parentId\":null}', '2019-05-28 22:29:57.047350', '43', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('339', '1', '2', 'Master.Web.Controllers.UserController', 'Index', '{}', '2019-05-28 22:32:27.722081', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('340', '1', '2', 'Master.Organizations.OrganizationAppService', 'GetTreeJson', '{}', '2019-05-28 22:32:29.448710', '210', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('341', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"User\"}', '2019-05-28 22:32:29.795403', '18', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('342', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:30.098149', '278', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('343', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"userName\\\",\\\"name\\\",\\\"organizationId\\\",\\\"isActive\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:30.496602', '116', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('344', '1', '2', 'Master.Web.Controllers.UserController', 'Index', '{}', '2019-05-28 22:32:37.717582', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('345', '1', '2', 'Master.Organizations.OrganizationAppService', 'GetTreeJson', '{}', '2019-05-28 22:32:38.158029', '8', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('346', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"User\"}', '2019-05-28 22:32:38.523277', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('347', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:38.608242', '28', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('348', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"userName\\\",\\\"name\\\",\\\"organizationId\\\",\\\"isActive\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:38.736176', '19', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('349', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":\"{\\\"organizationId\\\":1}\",\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:50.118449', '136', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('350', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"userName\\\",\\\"name\\\",\\\"organizationId\\\",\\\"isActive\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":\"{\\\"organizationId\\\":1}\",\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:50.329395', '56', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('351', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":\"{\\\"organizationId\\\":-1}\",\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:51.866563', '47', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('352', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"userName\\\",\\\"name\\\",\\\"organizationId\\\",\\\"isActive\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":\"{\\\"organizationId\\\":-1}\",\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:32:52.000357', '56', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('353', '1', '2', 'Master.Web.Controllers.RoleController', 'Index', '{}', '2019-05-28 22:33:07.143517', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('354', '1', '2', 'Master.Roles.RoleAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:33:07.999019', '106', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('355', '1', '2', 'Master.Web.Controllers.PermissionController', 'Assign', '{\"moduleKey\":\"Roles\",\"data\":\"2\"}', '2019-05-28 22:33:09.086951', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('356', '1', '2', 'Master.Menu.MenuAppService', 'GetMenuTreeJson', '{}', '2019-05-28 22:33:09.751039', '30', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('357', '1', '2', 'SkyNet.Master.Service.PermissionAppService', 'GetMenuDetailPermissions', '{}', '2019-05-28 22:33:09.879950', '5', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('358', '1', '2', 'SkyNet.Master.Service.PermissionAppService', 'LoadGrantedMenuPermissions', '{\"type\":\"Roles\",\"id\":2}', '2019-05-28 22:33:09.903389', '27', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('359', '1', '2', 'SkyNet.Master.Service.PermissionAppService', 'GrantAllPermissions', '{\"id\":2}', '2019-05-28 22:33:10.569430', '206', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('360', '1', '2', 'SkyNet.Master.Service.PermissionAppService', 'GetMenuDetailPermissions', '{}', '2019-05-28 22:33:10.881942', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('361', '1', '2', 'Master.Web.Controllers.PermissionController', 'Assign', '{\"moduleKey\":\"Roles\",\"data\":\"2\"}', '2019-05-28 22:33:11.881004', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('362', '1', '2', 'Master.Menu.MenuAppService', 'GetMenuTreeJson', '{}', '2019-05-28 22:33:12.291176', '4', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('363', '1', '2', 'SkyNet.Master.Service.PermissionAppService', 'LoadGrantedMenuPermissions', '{\"type\":\"Roles\",\"id\":2}', '2019-05-28 22:33:12.396648', '6', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('364', '1', '2', 'Master.Web.Controllers.UserController', 'Index', '{}', '2019-05-28 22:33:13.533411', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('365', '1', '2', 'Master.Organizations.OrganizationAppService', 'GetTreeJson', '{}', '2019-05-28 22:33:14.050032', '5', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('366', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"User\"}', '2019-05-28 22:33:14.278557', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('367', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:33:14.351802', '18', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('368', '1', '2', 'Master.Users.UserAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"userName\\\",\\\"name\\\",\\\"organizationId\\\",\\\"isActive\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:33:14.462157', '16', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('369', '1', '2', 'Master.Web.Controllers.UserController', 'Account', '{\"data\":\"2\"}', '2019-05-28 22:33:18.230857', '21', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('370', '1', '2', 'Master.Users.UserAppService', 'GetLoginProviders', '{}', '2019-05-28 22:33:19.302187', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('371', '1', '2', 'Master.Users.UserAppService', 'GetBindedLoginProviders', '{\"userId\":2}', '2019-05-28 22:33:19.377385', '98', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('372', '1', '2', 'Master.Web.Controllers.BusinessUserController', 'Assistant', '{}', '2019-05-28 22:36:02.633654', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('373', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"Assistant\"}', '2019-05-28 22:36:03.708891', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('374', '1', '2', 'Master.Users.AssistantAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:03.894445', '80', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('375', '1', '2', 'Master.Users.AssistantAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"workLocation\\\",\\\"userName\\\",\\\"email\\\",\\\"name\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:04.101484', '67', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('376', '1', '2', 'Master.Web.Controllers.AssistantController', 'Add', '{\"modulekey\":\"Assistant\"}', '2019-05-28 22:36:05.062458', '8', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('377', '1', '2', 'Master.Web.Mvc.Controllers.ModuleInfoController', 'Index', '{}', '2019-05-28 22:36:29.264559', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('378', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:29.978454', '55', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('379', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetModuleCountSummary', '{}', '2019-05-28 22:36:30.261668', '19', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('380', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":\"1=1\",\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":\"助理\",\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:33.124083', '39', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('381', '1', '2', 'Master.Module.ModuleInfoAppService', 'InitModuleInfo', '{\"moduleInfoIds\":[1]}', '2019-05-28 22:36:35.706213', '541', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('382', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":\"1=1\",\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":\"助理\",\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:36.325377', '8', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('383', '1', '2', 'Master.Web.Controllers.BusinessUserController', 'Assistant', '{}', '2019-05-28 22:36:37.713126', '0', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('384', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"Assistant\"}', '2019-05-28 22:36:38.432880', '1', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('385', '1', '2', 'Master.Users.AssistantAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:38.513938', '23', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('386', '1', '2', 'Master.Users.AssistantAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"workLocation\\\",\\\"userName\\\",\\\"email\\\",\\\"name\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-28 22:36:38.636013', '20', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('387', '1', '2', 'Master.Web.Controllers.AssistantController', 'Add', '{\"modulekey\":\"Assistant\"}', '2019-05-28 22:36:59.306729', '5', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('388', '1', '2', 'Master.Web.Mvc.Controllers.ModuleInfoController', 'Column', '{\"data\":\"Assistant\"}', '2019-05-28 22:37:05.883153', '27', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('389', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnInfos', '{\"moduleKey\":\"Assistant\"}', '2019-05-28 22:37:07.257229', '30', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('390', '1', '2', 'Master.Dictionaries.DictionaryAppService', 'GetAllKeysAsync', '{}', '2019-05-28 22:37:07.257229', '84', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('391', '1', '2', 'Master.Web.Controllers.HomeController', 'Index', '{}', '2019-05-30 12:37:12.316349', '925', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('392', '1', '2', 'Master.Web.Controllers.ManagerController', 'Index', '{}', '2019-05-30 12:37:59.801571', '11', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('393', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-30 12:38:00.451010', '1380', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('394', '1', '2', 'Master.Menu.MenuAppService', 'GetUserMenu', '{\"userIdentifier\":{\"tenantId\":1,\"userId\":2}}', '2019-05-30 12:38:02.270416', '95', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('395', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Index', '{}', '2019-05-30 12:38:07.685663', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('396', '1', '2', 'Master.Module.ModuleInfoAppService', 'GetColumnLayData', '{\"moduleKey\":\"CaseSource\"}', '2019-05-30 12:38:12.133099', '5', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('397', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-30 12:38:12.417290', '823', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('398', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-30 12:38:13.386077', '149', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('399', '1', '2', 'Master.Case.CaseSourceAppService', 'Freeze', '{\"ids\":[1]}', '2019-05-30 12:38:27.769442', '105', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('400', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":10,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":null,\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-30 12:38:27.974528', '22', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('401', '1', '2', 'Master.Case.CaseSourceAppService', 'GetPageResult', '{\"request\":{\"page\":1,\"limit\":20,\"where\":null,\"tableFilter\":null,\"filterField\":null,\"filterKey\":null,\"filterColumns\":\"[\\\"sourceSN\\\",\\\"cityId\\\",\\\"court1Id\\\",\\\"court2Id\\\",\\\"anYouId\\\",\\\"validDate\\\",\\\"sourceFile\\\",\\\"caseSourceStatus\\\",\\\"creatorUserId\\\",\\\"creationTime\\\",\\\"lastModifierUserId\\\",\\\"lastModificationTime\\\"]\",\"searchCondition\":null,\"filterSos\":null,\"searchKeys\":null,\"keyword\":null,\"orderField\":\"Id\",\"orderType\":\"desc\",\"moduleKey\":null}}', '2019-05-30 12:38:28.127854', '18', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('402', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{\"modulekey\":\"CaseSource\"}', '2019-05-30 12:38:29.798817', '4', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('403', '1', '2', 'Master.Case.CaseSourceAppService', 'GetById', '{\"primary\":1}', '2019-05-30 12:38:31.068397', '52', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('404', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentId', '{\"id\":2}', '2019-05-30 12:38:31.251997', '20', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('405', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-30 12:38:31.062537', '212', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('406', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-30 12:38:31.068397', '207', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('407', '1', '2', 'Master.Web.Controllers.CaseSourceController', 'Add', '{\"modulekey\":\"CaseSource\"}', '2019-05-30 12:38:44.581611', '4', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('408', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"纠纷属性\"}', '2019-05-30 12:38:45.412697', '8', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('409', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentName', '{\"name\":\"审判组织\"}', '2019-05-30 12:38:45.409768', '13', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('410', '1', '2', 'Master.Case.CaseSourceAppService', 'GetById', '{\"primary\":1}', '2019-05-30 12:38:45.426370', '6', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('411', '1', '2', 'Master.Case.TypeAppService', 'GetTypesByParentId', '{\"id\":2}', '2019-05-30 12:38:45.525006', '4', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);
INSERT INTO `auditlog` VALUES ('412', '1', '2', 'Master.Web.Controllers.AccountController', 'GMLogin', '{}', '2019-05-30 12:41:15.099109', '1', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', null, null, null, null);

-- ----------------------------
-- Table structure for basetree
-- ----------------------------
DROP TABLE IF EXISTS `basetree`;
CREATE TABLE `basetree` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `Discriminator` longtext,
  `ParentId` int(11) DEFAULT NULL,
  `Code` longtext,
  `DisplayName` longtext NOT NULL,
  `BriefCode` longtext,
  `Sort` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_BaseTree_CreatorUserId` (`CreatorUserId`),
  KEY `IX_BaseTree_DeleterUserId` (`DeleterUserId`),
  KEY `IX_BaseTree_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_BaseTree_ParentId` (`ParentId`),
  KEY `IX_BaseTree_TenantId` (`TenantId`),
  CONSTRAINT `FK_BaseTree_BaseTree_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `basetree` (`id`),
  CONSTRAINT `FK_BaseTree_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_BaseTree_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_BaseTree_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_BaseTree_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of basetree
-- ----------------------------
INSERT INTO `basetree` VALUES ('1', '2019-05-23 19:55:49.644943', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', null, '00001', '审判组织', 'a', '1');
INSERT INTO `basetree` VALUES ('2', '2019-05-23 19:56:00.472375', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '1', '00001.00001', '上海', 'shanghai', '1');
INSERT INTO `basetree` VALUES ('3', '2019-05-23 19:56:41.943353', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '1', '00001.00002', '北京', 'beijing', '2');
INSERT INTO `basetree` VALUES ('4', '2019-05-23 19:57:06.653686', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '2', '00001.00001.00001', '上海市松江区人民法院', 'court1', '1');
INSERT INTO `basetree` VALUES ('5', '2019-05-23 19:57:23.542253', '2', '2019-05-23 19:57:36.172304', '2', '\0', null, null, null, null, null, '1', 'BaseType', '2', '00001.00001.00002', '上海市浦东新区人民法院', 'court2', '2');
INSERT INTO `basetree` VALUES ('6', '2019-05-26 11:18:04.509300', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '1', '00001.00003', '长沙', null, '0');
INSERT INTO `basetree` VALUES ('7', '2019-05-26 11:20:57.413400', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', null, '00002', '专题', null, '1');
INSERT INTO `basetree` VALUES ('8', '2019-05-26 11:21:04.092367', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '7', '00002.00001', '专题一', null, '0');
INSERT INTO `basetree` VALUES ('9', '2019-05-26 11:21:24.140012', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', null, '00003', '纠纷属性', null, '0');
INSERT INTO `basetree` VALUES ('10', '2019-05-26 11:22:59.747199', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '9', '00003.00001', '房屋买卖纠纷（二手）', null, '0');
INSERT INTO `basetree` VALUES ('11', '2019-05-26 11:23:11.357020', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '9', '00003.00002', '房屋买卖纠纷（期房）', null, '0');
INSERT INTO `basetree` VALUES ('12', '2019-05-26 11:23:40.836667', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '9', '00003.00003', '商品房销售合同纠纷', null, '3');
INSERT INTO `basetree` VALUES ('13', '2019-05-26 11:25:30.609437', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '10', '00003.00001.00001', '定金纠纷', null, '0');
INSERT INTO `basetree` VALUES ('14', '2019-05-26 11:26:04.308973', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '13', '00003.00001.00001.00001', '纠纷原因', null, '0');
INSERT INTO `basetree` VALUES ('15', '2019-05-26 11:26:12.743867', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '13', '00003.00001.00001.00002', '判决结果', null, '0');
INSERT INTO `basetree` VALUES ('16', '2019-05-26 11:26:50.168156', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00001', '无权代理', null, '0');
INSERT INTO `basetree` VALUES ('17', '2019-05-26 11:26:56.165457', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00002', '无权处分', null, '0');
INSERT INTO `basetree` VALUES ('18', '2019-05-26 11:27:08.765550', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00001', '双倍退还', null, '0');
INSERT INTO `basetree` VALUES ('19', '2019-05-26 11:27:15.119309', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00002', '没收定金', null, '0');
INSERT INTO `basetree` VALUES ('20', '2019-05-27 22:13:52.889940', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00003', '违反协议', null, '3');
INSERT INTO `basetree` VALUES ('21', '2019-05-27 22:17:03.229320', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00004', '限购状态', null, '4');
INSERT INTO `basetree` VALUES ('22', '2019-05-27 22:17:33.321144', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00005', '未尽事宜', null, '5');
INSERT INTO `basetree` VALUES ('23', '2019-05-27 22:17:47.717683', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00006', '约定不明', null, '6');
INSERT INTO `basetree` VALUES ('24', '2019-05-27 22:18:30.229611', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00007', '履行不能', null, '6');
INSERT INTO `basetree` VALUES ('25', '2019-05-27 22:18:51.067144', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00008', '定金认定', null, '10');
INSERT INTO `basetree` VALUES ('26', '2019-05-27 22:19:03.997957', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00009', '其他', null, '20');
INSERT INTO `basetree` VALUES ('27', '2019-05-27 22:20:01.360497', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '14', '00003.00001.00001.00001.00010', '定金返还', null, '20');
INSERT INTO `basetree` VALUES ('28', '2019-05-27 22:20:19.962822', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00003', '定金返还', null, '3');
INSERT INTO `basetree` VALUES ('29', '2019-05-27 22:21:20.025384', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00004', '赔偿实际损失', null, '20');
INSERT INTO `basetree` VALUES ('30', '2019-05-27 22:21:35.830036', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00005', '赔偿违约金', null, '30');
INSERT INTO `basetree` VALUES ('31', '2019-05-27 22:21:51.553639', '2', null, null, '\0', null, null, null, null, null, '1', 'BaseType', '15', '00003.00001.00001.00002.00006', '其他', null, '40');

-- ----------------------------
-- Table structure for basetype
-- ----------------------------
DROP TABLE IF EXISTS `basetype`;
CREATE TABLE `basetype` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `Sort` int(11) NOT NULL,
  `Discriminator` longtext,
  `Code` longtext,
  `Name` longtext NOT NULL,
  `BriefName` longtext,
  `IsActive` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_BaseType_CreatorUserId` (`CreatorUserId`),
  KEY `IX_BaseType_DeleterUserId` (`DeleterUserId`),
  KEY `IX_BaseType_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_BaseType_TenantId` (`TenantId`),
  CONSTRAINT `FK_BaseType_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_BaseType_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_BaseType_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_BaseType_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of basetype
-- ----------------------------

-- ----------------------------
-- Table structure for casecard
-- ----------------------------
DROP TABLE IF EXISTS `casecard`;
CREATE TABLE `casecard` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `Title` longtext,
  `Content` longtext,
  `CaseInitialId` int(11) NOT NULL,
  `Status` longtext,
  `IsActive` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseCard_CaseInitialId` (`CaseInitialId`),
  KEY `IX_CaseCard_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseCard_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseCard_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseCard_TenantId` (`TenantId`),
  CONSTRAINT `FK_CaseCard_CaseInitial_CaseInitialId` FOREIGN KEY (`CaseInitialId`) REFERENCES `caseinitial` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseCard_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseCard_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseCard_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseCard_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of casecard
-- ----------------------------

-- ----------------------------
-- Table structure for casefile
-- ----------------------------
DROP TABLE IF EXISTS `casefile`;
CREATE TABLE `casefile` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `CaseInitialId` int(11) NOT NULL,
  `Status` longtext,
  `IsActive` bit(1) NOT NULL,
  `Title` longtext,
  `Content` longtext,
  `MediaPath` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseFile_CaseInitialId` (`CaseInitialId`),
  KEY `IX_CaseFile_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseFile_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseFile_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseFile_TenantId` (`TenantId`),
  CONSTRAINT `FK_CaseFile_CaseInitial_CaseInitialId` FOREIGN KEY (`CaseInitialId`) REFERENCES `caseinitial` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseFile_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseFile_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseFile_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseFile_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of casefile
-- ----------------------------

-- ----------------------------
-- Table structure for caseinitial
-- ----------------------------
DROP TABLE IF EXISTS `caseinitial`;
CREATE TABLE `caseinitial` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `CaseSourceId` int(11) NOT NULL,
  `Title` longtext,
  `Introduction` longtext,
  `Law` longtext,
  `Experience` longtext,
  `LawyerOpinion` longtext,
  `Status` longtext,
  `PublisDate` datetime(6) DEFAULT NULL,
  `ReadNumber` int(11) NOT NULL,
  `PraiseNumber` int(11) NOT NULL,
  `BeatNumber` int(11) NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CaseStatus` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseInitial_CaseSourceId` (`CaseSourceId`),
  KEY `IX_CaseInitial_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseInitial_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseInitial_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseInitial_TenantId` (`TenantId`),
  CONSTRAINT `FK_CaseInitial_CaseSource_CaseSourceId` FOREIGN KEY (`CaseSourceId`) REFERENCES `casesource` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseInitial_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseInitial_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseInitial_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseInitial_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of caseinitial
-- ----------------------------

-- ----------------------------
-- Table structure for casekey
-- ----------------------------
DROP TABLE IF EXISTS `casekey`;
CREATE TABLE `casekey` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `KeyName` longtext,
  `KeyValue` longtext,
  `KeyNodeId` int(11) NOT NULL,
  `CaseInitialId` int(11) DEFAULT NULL,
  `CaseFineId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseKey_CaseFineId` (`CaseFineId`),
  KEY `IX_CaseKey_CaseInitialId` (`CaseInitialId`),
  KEY `IX_CaseKey_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseKey_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseKey_KeyNodeId` (`KeyNodeId`),
  KEY `IX_CaseKey_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseKey_TenantId` (`TenantId`),
  CONSTRAINT `FK_CaseKey_BaseTree_KeyNodeId` FOREIGN KEY (`KeyNodeId`) REFERENCES `basetree` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseKey_CaseFile_CaseFineId` FOREIGN KEY (`CaseFineId`) REFERENCES `casefile` (`id`),
  CONSTRAINT `FK_CaseKey_CaseInitial_CaseInitialId` FOREIGN KEY (`CaseInitialId`) REFERENCES `caseinitial` (`id`),
  CONSTRAINT `FK_CaseKey_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseKey_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseKey_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseKey_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of casekey
-- ----------------------------

-- ----------------------------
-- Table structure for casesource
-- ----------------------------
DROP TABLE IF EXISTS `casesource`;
CREATE TABLE `casesource` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `SourceSN` longtext,
  `CityId` int(11) DEFAULT NULL,
  `Court1Id` int(11) DEFAULT NULL,
  `Court2Id` int(11) DEFAULT NULL,
  `AnYouId` int(11) DEFAULT NULL,
  `ValidDate` datetime(6) NOT NULL,
  `SourceFile` longtext,
  `IsActive` bit(1) NOT NULL,
  `Status` longtext,
  `CaseSourceStatus` int(11) NOT NULL,
  `OwerId` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseSource_AnYouId` (`AnYouId`),
  KEY `IX_CaseSource_CityId` (`CityId`),
  KEY `IX_CaseSource_Court1Id` (`Court1Id`),
  KEY `IX_CaseSource_Court2Id` (`Court2Id`),
  KEY `IX_CaseSource_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseSource_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseSource_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseSource_TenantId` (`TenantId`),
  KEY `IX_CaseSource_OwerId` (`OwerId`),
  CONSTRAINT `FK_CaseSource_BaseTree_AnYouId` FOREIGN KEY (`AnYouId`) REFERENCES `basetree` (`id`),
  CONSTRAINT `FK_CaseSource_BaseTree_CityId` FOREIGN KEY (`CityId`) REFERENCES `basetree` (`id`),
  CONSTRAINT `FK_CaseSource_BaseTree_Court1Id` FOREIGN KEY (`Court1Id`) REFERENCES `basetree` (`id`),
  CONSTRAINT `FK_CaseSource_BaseTree_Court2Id` FOREIGN KEY (`Court2Id`) REFERENCES `basetree` (`id`),
  CONSTRAINT `FK_CaseSource_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseSource_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseSource_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseSource_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseSource_User_OwerId` FOREIGN KEY (`OwerId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of casesource
-- ----------------------------
INSERT INTO `casesource` VALUES ('1', '2019-05-27 19:47:02.519478', '2', '2019-05-30 12:38:27.819248', '2', '\0', null, null, null, null, '{\"LawyerFirms\": [{\"lawyer\": \"qqq\", \"FirmName\": \"qqqq\"}], \"TrialPeople\": [{\"Name\": \"a\", \"TrialRole\": 0}]}', '1', '111', '2', '4', '5', '10', '2019-05-30 00:00:00.000000', '/files/2019/05/27/6f9191a7-ace2-4b9b-b87f-2b3c86ee7a1f.pdf', '', null, '-1', null);

-- ----------------------------
-- Table structure for casesourcehistory
-- ----------------------------
DROP TABLE IF EXISTS `casesourcehistory`;
CREATE TABLE `casesourcehistory` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `CaseSourceId` int(11) NOT NULL,
  `Reason` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_CaseSourceHistory_CaseSourceId` (`CaseSourceId`),
  KEY `IX_CaseSourceHistory_CreatorUserId` (`CreatorUserId`),
  KEY `IX_CaseSourceHistory_DeleterUserId` (`DeleterUserId`),
  KEY `IX_CaseSourceHistory_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_CaseSourceHistory_TenantId` (`TenantId`),
  CONSTRAINT `FK_CaseSourceHistory_CaseSource_CaseSourceId` FOREIGN KEY (`CaseSourceId`) REFERENCES `casesource` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseSourceHistory_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CaseSourceHistory_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseSourceHistory_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_CaseSourceHistory_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of casesourcehistory
-- ----------------------------

-- ----------------------------
-- Table structure for columninfo
-- ----------------------------
DROP TABLE IF EXISTS `columninfo`;
CREATE TABLE `columninfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `TenantId` int(11) NOT NULL,
  `ModuleInfoId` int(11) NOT NULL,
  `ColumnType` int(11) NOT NULL,
  `EnableFieldPermission` bit(1) NOT NULL,
  `ControlFormat` longtext,
  `Sort` int(11) NOT NULL,
  `ColumnKey` longtext,
  `ColumnName` longtext,
  `Templet` longtext,
  `MaxFileNumber` int(11) NOT NULL,
  `IsInterColumn` bit(1) NOT NULL,
  `IsSystemColumn` bit(1) NOT NULL,
  `DisplayFormat` longtext,
  `DefaultValue` longtext,
  `VerifyRules` longtext,
  `Renderer` longtext,
  `ValuePath` longtext,
  `DisplayPath` longtext,
  `DictionaryName` longtext,
  `CustomizeControl` longtext,
  `ControlParameter` longtext,
  `IsShownInList` bit(1) NOT NULL,
  `IsShownInAdd` bit(1) NOT NULL,
  `IsShownInEdit` bit(1) NOT NULL,
  `IsShownInMultiEdit` bit(1) NOT NULL,
  `IsShownInAdvanceSearch` bit(1) NOT NULL,
  `IsShownInView` bit(1) NOT NULL,
  `IsEnableSort` bit(1) NOT NULL,
  `RelativeDataType` int(11) NOT NULL,
  `RelativeDataString` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_ColumnInfo_ModuleInfoId` (`ModuleInfoId`),
  CONSTRAINT `FK_ColumnInfo_ModuleInfo_ModuleInfoId` FOREIGN KEY (`ModuleInfoId`) REFERENCES `moduleinfo` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of columninfo
-- ----------------------------
INSERT INTO `columninfo` VALUES ('11', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '1', 'Name', '姓名', null, '1', '', '\0', null, null, 'required', null, 'Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('12', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '0', 'UserName', '登录用户名', null, '1', '', '\0', null, null, 'required', null, 'UserName', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('13', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '0', 'WorkLocation', '律师事务所', null, '1', '', '\0', null, null, 'required', null, 'WorkLocation', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('14', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '0', 'OrganizationId', '所属组织', '{{d.organizationId_display?d.organizationId_display:\'\'}}', '1', '', '\0', null, null, null, 'lay-departchoose', 'OrganizationId', 'Organization.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('15', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('16', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '3', '\0', 'datetime', '999', 'CreationTime', '创建时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'CreationTime', null, null, null, null, '', '\0', '\0', '', '', '', '', '0', null);
INSERT INTO `columninfo` VALUES ('17', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('18', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, null, '1', '2', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('19', '2019-05-23 19:49:12.849417', null, null, null, '\0', null, null, '{\"fixed\":\"right\"}', '1', '2', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('20', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '1', 'Name', '姓名', null, '1', '', '\0', null, null, null, null, 'Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('21', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '0', 'WorkLocation', '律师事务所', null, '1', '', '\0', null, null, null, null, 'WorkLocation', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('22', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '0', 'Email', '有效电子邮箱', null, '1', '', '\0', null, null, null, null, 'Email', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('23', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '0', 'OrganizationId', '所属组织', '{{d.organizationId_display?d.organizationId_display:\'\'}}', '1', '', '\0', null, null, null, 'lay-departchoose', 'OrganizationId', 'Organization.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('24', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('25', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '3', '\0', 'datetime', '999', 'CreationTime', '创建时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'CreationTime', null, null, null, null, '', '\0', '\0', '', '', '', '', '0', null);
INSERT INTO `columninfo` VALUES ('26', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('27', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, null, '1', '3', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('28', '2019-05-23 19:49:12.913873', null, null, null, '\0', null, null, '{\"fixed\":\"right\"}', '1', '3', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('29', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'SourceSN', '案号', null, '1', '', '\0', null, null, null, null, 'SourceSN', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('30', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'CityId', '城市', '{{d.cityId_display||\'\'}}', '1', '', '\0', null, null, null, null, 'CityId', 'City.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('31', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'Court1Id', '一审法院', '{{d.court1Id_display||\'\'}}', '1', '', '\0', null, null, null, null, 'Court1Id', 'Court1.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('32', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'Court2Id', '二审法院', '{{d.court2Id_display||\'\'}}', '1', '', '\0', null, null, null, null, 'Court2Id', 'Court2.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('33', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'AnYouId', '案由', '{{d.anYouId_display||\'\'}}', '1', '', '\0', null, null, null, null, 'AnYouId', 'AnYou.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('34', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '3', '\0', null, '0', 'ValidDate', '生效日期', null, '1', '', '\0', null, null, null, null, 'ValidDate', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('35', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '0', 'SourceFile', '判例原件', '<button class=\"layui-btn layui-btn-xs\" onclick=\"showPdf(\'{{d.sourceFile}}\',\'{{d.sourceSN}}\')\">查看</button>', '1', '', '\0', null, null, null, null, 'SourceFile', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('36', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '5', '\0', null, '0', 'CaseSourceStatus', '状态', '{{d.caseSourceStatus_display}}', '1', '', '\0', null, null, null, null, 'CaseSourceStatus', null, 'Master.Case.CaseSourceStatus', null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('37', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('38', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '3', '\0', 'datetime', '999', 'CreationTime', '创建时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'CreationTime', null, null, null, null, '', '\0', '\0', '', '', '', '', '0', null);
INSERT INTO `columninfo` VALUES ('39', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('40', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, null, '1', '4', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('41', '2019-05-23 19:49:12.962703', null, null, null, '\0', null, null, '{\"fixed\":\"right\"}', '1', '4', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('42', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'Avata', '微信头像', null, '1', '', '\0', null, null, null, null, 'Avata', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('43', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'NickName', '昵称', null, '1', '', '\0', null, null, null, null, 'NickName', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('44', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'Name', '姓名', null, '1', '', '\0', null, null, null, null, 'Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('45', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'WorkLocation', '律师事务所', null, '1', '', '\0', null, null, null, null, 'WorkLocation', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('46', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'Email', '邮箱', null, '1', '', '\0', null, null, null, null, 'Email', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('47', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '3', '\0', null, '0', 'CreationTime', '申请时间', null, '1', '', '\0', null, null, null, null, 'CreationTime', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('48', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '0', 'Remarks', '留言', null, '1', '', '\0', null, null, null, null, 'Remarks', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('49', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('50', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('51', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, null, '1', '5', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('52', '2019-05-23 19:49:13.021299', null, null, null, '\0', null, null, '{\"fixed\":\"right\"}', '1', '5', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('53', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '1', '\0', null, '0', 'UserName', '账号', null, '1', '', '\0', null, null, null, null, 'UserName', null, null, null, null, '', '\0', '\0', '\0', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('54', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '1', '\0', null, '1', 'Name', '姓名', null, '1', '', '\0', null, null, 'required', null, 'Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('55', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '1', '\0', null, '2', 'OrganizationId', '所属组织', '{{d.organizationId_display?d.organizationId_display:\'\'}}', '1', '', '\0', null, null, null, 'lay-departchoose', 'OrganizationId', 'Organization.DisplayName', null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('56', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '6', '\0', null, '3', 'IsActive', '激活状态', '{{d.isActive?\'<span class=\"layui-badge layui-bg-green\">激活</span>\':\'<span class=\"layui-badge layui-bg-gray\">冻结</span>\'}}', '1', '', '\0', null, null, null, null, 'IsActive', null, null, null, null, '', '\0', '\0', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('57', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('58', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '3', '\0', 'datetime', '999', 'CreationTime', '创建时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'CreationTime', null, null, null, null, '', '\0', '\0', '', '', '', '', '0', null);
INSERT INTO `columninfo` VALUES ('59', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('60', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, null, '1', '6', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('61', '2019-05-23 19:49:13.084778', null, null, null, '\0', null, null, '{\"fixed\":\"right\",\"width\":\"240\"}', '1', '6', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('62', '2019-05-23 19:49:13.090637', null, null, null, '\0', null, null, null, '1', '6', '11', '\0', null, '0', 'RoleNames', '角色', null, '1', '', '\0', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '\0', '\0', '', '\0', '1', null);
INSERT INTO `columninfo` VALUES ('63', '2019-05-26 11:13:09.114168', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'SourceSN', '案号', null, '1', '', '\0', null, null, null, null, 'CaseSource.SourceSN', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('64', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'City', '城市', null, '1', '', '\0', null, null, null, null, 'CaseSource.City.DisplayName', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('65', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'AnYou', '案由', null, '1', '', '\0', null, null, null, null, 'CaseSource.AnYou.DisplayName', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('66', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'Title', '标题', null, '1', '', '\0', null, null, null, null, 'Title', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('67', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'Processor', '加工人', null, '1', '', '\0', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('68', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'PublisDate', '发布日期', null, '1', '', '\0', null, null, null, null, 'PublisDate', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('69', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'PraiseNumber', '点赞数', null, '1', '', '\0', null, null, null, null, 'PraiseNumber', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('70', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '1', '\0', null, '0', 'BeatNumber', '拍砖数', null, '1', '', '\0', null, null, null, null, 'BeatNumber', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('71', '2019-05-26 11:13:09.115144', null, null, null, '\0', null, null, null, '1', '19', '5', '\0', null, '0', 'CaseStatus', '状态', null, '1', '', '\0', null, null, null, null, 'CaseStatus', null, 'Master.Case.CaseStatus', null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('72', '2019-05-28 22:36:36.050953', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '1', 'Name', '姓名', null, '1', '', '\0', null, null, 'required', null, 'Name', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('73', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '0', 'WorkLocation', '律师事务所', null, '1', '', '\0', null, null, 'required', null, 'WorkLocation', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('74', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '0', 'UserName', '登录用户名', null, '1', '', '\0', null, null, 'required', null, 'UserName', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('75', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '0', 'Email', '有效电子邮箱', null, '1', '', '\0', null, null, 'required', null, 'Email', null, null, null, null, '', '', '', '', '', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('76', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '0', 'InputCaseNumber', '判例汇总', null, '1', '', '\0', null, null, null, null, 'Property', null, null, null, null, '', '\0', '\0', '', '\0', '', '', '1', null);
INSERT INTO `columninfo` VALUES ('77', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '999', 'CreatorUserId', '创建人', null, '1', '', '', null, null, null, null, 'CreatorUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('78', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '3', '\0', 'datetime', '999', 'CreationTime', '创建时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'CreationTime', null, null, null, null, '', '\0', '\0', '', '', '', '', '0', null);
INSERT INTO `columninfo` VALUES ('79', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '1', '\0', null, '999', 'LastModifierUserId', '修改人', null, '1', '', '', null, null, null, null, 'LastModifierUser.Name', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('80', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, null, '1', '20', '3', '\0', 'datetime', '999', 'LastModificationTime', '修改时间', null, '1', '', '', 'yyyy-MM-dd HH:mm:ss', null, null, null, 'LastModificationTime', null, null, null, null, '', '\0', '\0', '', '', '', '\0', '0', null);
INSERT INTO `columninfo` VALUES ('81', '2019-05-28 22:36:36.051929', '2', null, null, '\0', null, null, '{\"fixed\":\"right\"}', '1', '20', '14', '\0', null, '1000', 'Operation', '操作', null, '1', '', '', null, null, null, null, null, null, null, null, null, '', '\0', '\0', '', '', '\0', '\0', '0', null);

-- ----------------------------
-- Table structure for dictionary
-- ----------------------------
DROP TABLE IF EXISTS `dictionary`;
CREATE TABLE `dictionary` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `DictionaryName` longtext,
  `DictionaryContent` longtext,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Dictionary_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Dictionary_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Dictionary_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Dictionary_TenantId` (`TenantId`),
  CONSTRAINT `FK_Dictionary_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Dictionary_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Dictionary_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Dictionary_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of dictionary
-- ----------------------------

-- ----------------------------
-- Table structure for edition
-- ----------------------------
DROP TABLE IF EXISTS `edition`;
CREATE TABLE `edition` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `Name` varchar(32) NOT NULL,
  `DisplayName` varchar(64) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of edition
-- ----------------------------

-- ----------------------------
-- Table structure for emaillog
-- ----------------------------
DROP TABLE IF EXISTS `emaillog`;
CREATE TABLE `emaillog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `ToEmail` longtext,
  `Title` longtext,
  `Content` longtext,
  `Message` longtext,
  `Success` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of emaillog
-- ----------------------------

-- ----------------------------
-- Table structure for featuresetting
-- ----------------------------
DROP TABLE IF EXISTS `featuresetting`;
CREATE TABLE `featuresetting` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` varchar(2000) NOT NULL,
  `Discriminator` longtext NOT NULL,
  `EditionId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_FeatureSetting_EditionId` (`EditionId`),
  CONSTRAINT `FK_FeatureSetting_Edition_EditionId` FOREIGN KEY (`EditionId`) REFERENCES `edition` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of featuresetting
-- ----------------------------

-- ----------------------------
-- Table structure for file
-- ----------------------------
DROP TABLE IF EXISTS `file`;
CREATE TABLE `file` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `FileName` longtext,
  `FileSize` decimal(65,30) NOT NULL,
  `FilePath` longtext,
  `TenantId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_File_CreatorUserId` (`CreatorUserId`),
  KEY `IX_File_DeleterUserId` (`DeleterUserId`),
  KEY `IX_File_LastModifierUserId` (`LastModifierUserId`),
  CONSTRAINT `FK_File_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_File_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_File_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of file
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_aggregatedcounter
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_aggregatedcounter`;
CREATE TABLE `hangfire_aggregatedcounter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` int(11) NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_CounterAggregated_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_aggregatedcounter
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_counter
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_counter`;
CREATE TABLE `hangfire_counter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` int(11) NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Counter_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_counter
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_distributedlock
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_distributedlock`;
CREATE TABLE `hangfire_distributedlock` (
  `Resource` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_distributedlock
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_hash
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_hash`;
CREATE TABLE `hangfire_hash` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Field` varchar(40) NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_hash
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_job
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_job`;
CREATE TABLE `hangfire_job` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StateId` int(11) DEFAULT NULL,
  `StateName` varchar(20) DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Job_StateName` (`StateName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_job
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_jobparameter
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_jobparameter`;
CREATE TABLE `hangfire_jobparameter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_JobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_JobParameter_Job` (`JobId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_jobparameter
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_jobqueue
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_jobqueue`;
CREATE TABLE `hangfire_jobqueue` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Queue` varchar(50) NOT NULL,
  `FetchedAt` datetime DEFAULT NULL,
  `FetchToken` varchar(36) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_JobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_jobqueue
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_jobstate
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_jobstate`;
CREATE TABLE `hangfire_jobstate` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Reason` varchar(100) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_JobState_Job` (`JobId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_jobstate
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_list
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_list`;
CREATE TABLE `hangfire_list` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_list
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_server
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_server`;
CREATE TABLE `hangfire_server` (
  `Id` varchar(100) NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_server
-- ----------------------------
INSERT INTO `hangfire_server` VALUES ('10_105_60_63:d9eb28d9-695b-4b56-9cba-fef597d0e23e', '{\"WorkerCount\":10,\"Queues\":[\"default\"],\"StartedAt\":\"2019-05-30T00:12:12.9853673Z\"}', '2019-05-30 04:49:09');

-- ----------------------------
-- Table structure for hangfire_set
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_set`;
CREATE TABLE `hangfire_set` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` varchar(100) NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Set_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_set
-- ----------------------------

-- ----------------------------
-- Table structure for hangfire_state
-- ----------------------------
DROP TABLE IF EXISTS `hangfire_state`;
CREATE TABLE `hangfire_state` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Reason` varchar(100) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangFire_State_Job` (`JobId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of hangfire_state
-- ----------------------------

-- ----------------------------
-- Table structure for modulebutton
-- ----------------------------
DROP TABLE IF EXISTS `modulebutton`;
CREATE TABLE `modulebutton` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `ModuleInfoId` int(11) NOT NULL,
  `ButtonKey` longtext,
  `ButtonName` longtext,
  `ButtonClass` longtext,
  `TitleTemplet` longtext,
  `ButtonActionType` int(11) NOT NULL,
  `ButtonType` int(11) NOT NULL,
  `ButtonActionUrl` longtext,
  `ButtonActionParam` longtext,
  `ConfirmMsg` longtext,
  `ButtonScript` longtext,
  `Sort` int(11) NOT NULL,
  `IsEnabled` bit(1) NOT NULL,
  `RequirePermission` bit(1) NOT NULL,
  `ClientShowCondition` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_ModuleButton_CreatorUserId` (`CreatorUserId`),
  KEY `IX_ModuleButton_DeleterUserId` (`DeleterUserId`),
  KEY `IX_ModuleButton_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_ModuleButton_ModuleInfoId` (`ModuleInfoId`),
  KEY `IX_ModuleButton_TenantId` (`TenantId`),
  CONSTRAINT `FK_ModuleButton_ModuleInfo_ModuleInfoId` FOREIGN KEY (`ModuleInfoId`) REFERENCES `moduleinfo` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ModuleButton_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ModuleButton_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleButton_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleButton_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of modulebutton
-- ----------------------------
INSERT INTO `modulebutton` VALUES ('6', '2019-05-23 19:49:12.855277', null, null, null, '\0', null, null, null, null, null, '1', '2', 'Delete', '删除', 'layui-btn-danger', null, '1', '3', 'abp.services.app.charger.deleteEntity', null, '确认删除？', null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('7', '2019-05-23 19:49:12.855277', null, null, null, '\0', null, null, null, null, null, '1', '2', 'Add', '添加', '', null, '2', '4', '/Charger/Add', '{\"area\": [\"80%\", \"90%\"]}', null, null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('8', '2019-05-23 19:49:12.855277', null, null, null, '\0', null, null, null, null, null, '1', '2', 'Edit', '编辑', '', null, '2', '1', '/Charger/Edit', '{\"area\": [\"80%\", \"90%\"]}', null, null, '0', '', '', null);
INSERT INTO `modulebutton` VALUES ('9', '2019-05-23 19:49:12.863090', null, null, null, '\0', null, null, null, null, null, '1', '2', 'Freeze', '冻结', 'layui-danger', null, '1', '1', 'abp.services.app.user.freeze', null, '确认冻结此用户？', null, '1', '', '', 'd.isActive');
INSERT INTO `modulebutton` VALUES ('10', '2019-05-23 19:49:12.863090', null, null, null, '\0', null, null, null, null, null, '1', '2', 'UnFreeze', '解冻', '', null, '1', '1', 'abp.services.app.user.unFreeze', null, '确认解冻此用户？', null, '2', '', '', '!d.isActive');
INSERT INTO `modulebutton` VALUES ('11', '2019-05-23 19:49:12.919732', null, null, null, '\0', null, null, null, null, null, '1', '3', 'Delete', '删除', 'layui-btn-danger', null, '1', '3', 'abp.services.app.miner.deleteEntity', null, '确认删除？', null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('12', '2019-05-23 19:49:12.925592', null, null, null, '\0', null, null, null, null, null, '1', '3', 'Freeze', '冻结', 'layui-danger', null, '1', '1', 'abp.services.app.user.freeze', null, '用户冻结后，将不能再登陆平台，但他的数据将会被保留，确定继续吗？', null, '1', '', '', 'd.isActive');
INSERT INTO `modulebutton` VALUES ('13', '2019-05-23 19:49:12.925592', null, null, null, '\0', null, null, null, null, null, '1', '3', 'UnFreeze', '解冻', '', null, '1', '1', 'abp.services.app.user.unFreeze', null, '确认解冻此用户？', null, '2', '', '', '!d.isActive');
INSERT INTO `modulebutton` VALUES ('14', '2019-05-23 19:49:12.968562', null, '2019-05-26 11:13:09.402265', null, '\0', null, null, null, null, null, '1', '4', 'Delete', '删除', 'layui-btn-danger', null, '1', '3', 'abp.services.app.caseSource.deleteEntity', null, '确认删除？', null, '3', '', '', 'd.caseSourceStatus==-1');
INSERT INTO `modulebutton` VALUES ('15', '2019-05-23 19:49:12.968562', null, '2019-05-26 11:13:09.404218', null, '\0', null, null, null, null, null, '1', '4', 'Edit', '编辑', '', null, '2', '1', '/CaseSource/Add', '{\"area\": [\"100%\",\"100%\"]}', null, null, '0', '', '', 'd.caseSourceStatus==-1');
INSERT INTO `modulebutton` VALUES ('16', '2019-05-23 19:49:13.027158', null, null, null, '\0', null, null, null, null, null, '1', '5', 'Delete', '拒绝', 'layui-btn-danger', null, '1', '3', 'abp.services.app.newMiner.deleteEntity', null, '您确定拒绝这些用户的申请吗？', null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('17', '2019-05-23 19:49:13.033018', null, null, null, '\0', null, null, null, null, null, '1', '5', 'Verify', '审核', 'layui-danger', null, '1', '2', 'abp.services.app.newMiner.verify', null, '审核通过后，这些用户将可以通过微信账户登录案例工厂，确定继续吗？', null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('18', '2019-05-23 19:49:13.091614', null, null, null, '\0', null, null, null, null, null, '1', '6', 'Delete', '删除', 'layui-btn-danger', null, '1', '3', 'abp.services.app.user.deleteEntity', null, '确认删除？', null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('19', '2019-05-23 19:49:13.091614', null, null, null, '\0', null, null, null, null, null, '1', '6', 'Add', '添加', '', null, '2', '4', '/User/Add', '{\"area\": [\"80%\", \"90%\"]}', null, null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('20', '2019-05-23 19:49:13.091614', null, null, null, '\0', null, null, null, null, null, '1', '6', 'Edit', '编辑', '', null, '2', '1', '/User/Edit', '{\"area\": [\"80%\", \"90%\"]}', null, null, '0', '', '', null);
INSERT INTO `modulebutton` VALUES ('21', '2019-05-23 19:49:13.097474', null, null, null, '\0', null, null, null, null, null, '1', '6', 'Permission', '权限', '', null, '2', '1', '/Permission/Assign', '{\"btn\": []}', null, null, '4', '', '', null);
INSERT INTO `modulebutton` VALUES ('22', '2019-05-23 19:49:13.097474', null, null, null, '\0', null, null, null, null, null, '1', '6', 'Account', '账号', '', null, '2', '1', '/User/Account', '{\"area\": [\"100%\", \"100%\"]}', null, null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('23', '2019-05-26 11:13:09.127840', null, null, null, '\0', null, null, null, null, null, '1', '19', 'Back', '退回重做', 'layui-danger', null, '1', '2', 'abp.services.app.case.back', null, '确认退回此案例？', null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('24', '2019-05-26 11:13:09.127840', null, null, null, '\0', null, null, null, null, null, '1', '19', 'Down', '下架', 'layui-danger', null, '1', '2', 'abp.services.app.case.down', null, '确认下架此案例？', null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('25', '2019-05-26 11:13:09.127840', null, null, null, '\0', null, null, null, null, null, '1', '19', 'Recommand', '推荐', null, null, '1', '2', 'abp.services.app.case.recommand', null, '确认推荐此案例？', null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('26', '2019-05-26 11:13:09.390546', null, null, null, '\0', null, null, null, null, null, '1', '4', 'Freeze', '下架', 'layui-btn-danger', null, '1', '3', 'abp.services.app.caseSource.freeze', null, '确认下架此判例？', null, '1', '', '', 'd.caseSourceStatus==0');
INSERT INTO `modulebutton` VALUES ('27', '2019-05-26 11:13:09.390546', null, null, null, '\0', null, null, null, null, null, '1', '4', 'UnFreeze', '上架', '', null, '1', '3', 'abp.services.app.caseSource.unFreeze', null, '确认上架此判例？', null, '2', '', '', 'd.caseSourceStatus==-1');
INSERT INTO `modulebutton` VALUES ('28', '2019-05-28 22:36:36.059742', '2', null, null, '\0', null, null, null, null, null, '1', '20', 'Delete', '删除', 'layui-btn-danger', null, '1', '3', 'abp.services.app.assistant.deleteEntity', null, '确认删除？', null, '3', '', '', null);
INSERT INTO `modulebutton` VALUES ('29', '2019-05-28 22:36:36.059742', '2', null, null, '\0', null, null, null, null, null, '1', '20', 'Add', '添加', '', null, '2', '4', '/Assistant/Add', '{\"area\": [\"80%\", \"90%\"]}', null, null, '1', '', '', null);
INSERT INTO `modulebutton` VALUES ('30', '2019-05-28 22:36:36.059742', '2', null, null, '\0', null, null, null, null, null, '1', '20', 'Edit', '编辑', '', null, '2', '1', '/Assistant/Edit', '{\"area\": [\"80%\", \"90%\"]}', null, null, '0', '', '', null);
INSERT INTO `modulebutton` VALUES ('31', '2019-05-28 22:36:36.065602', '2', null, null, '\0', null, null, null, null, null, '1', '20', 'Freeze', '冻结', 'layui-danger', null, '1', '1', 'abp.services.app.user.freeze', null, '确认冻结此用户？', null, '1', '', '', 'd.isActive');
INSERT INTO `modulebutton` VALUES ('32', '2019-05-28 22:36:36.065602', '2', null, null, '\0', null, null, null, null, null, '1', '20', 'UnFreeze', '解冻', '', null, '1', '1', 'abp.services.app.user.unFreeze', null, '确认解冻此用户？', null, '2', '', '', '!d.isActive');

-- ----------------------------
-- Table structure for moduledata
-- ----------------------------
DROP TABLE IF EXISTS `moduledata`;
CREATE TABLE `moduledata` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `ModuleInfoId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ModuleData_CreatorUserId` (`CreatorUserId`),
  KEY `IX_ModuleData_DeleterUserId` (`DeleterUserId`),
  KEY `IX_ModuleData_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_ModuleData_ModuleInfoId` (`ModuleInfoId`),
  CONSTRAINT `FK_ModuleData_ModuleInfo_ModuleInfoId` FOREIGN KEY (`ModuleInfoId`) REFERENCES `moduleinfo` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ModuleData_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleData_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleData_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of moduledata
-- ----------------------------

-- ----------------------------
-- Table structure for moduleinfo
-- ----------------------------
DROP TABLE IF EXISTS `moduleinfo`;
CREATE TABLE `moduleinfo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `TenantId` int(11) NOT NULL,
  `IsInterModule` bit(1) NOT NULL,
  `RequiredFeature` longtext,
  `ModuleKey` longtext,
  `EntityFullName` longtext,
  `ModuleName` longtext,
  `DefaultLimit` int(11) NOT NULL,
  `Limits` longtext,
  `SortField` longtext,
  `SortType` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ModuleInfo_CreatorUserId` (`CreatorUserId`),
  KEY `IX_ModuleInfo_DeleterUserId` (`DeleterUserId`),
  KEY `IX_ModuleInfo_LastModifierUserId` (`LastModifierUserId`),
  CONSTRAINT `FK_ModuleInfo_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleInfo_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_ModuleInfo_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of moduleinfo
-- ----------------------------
INSERT INTO `moduleinfo` VALUES ('2', '2019-05-23 19:49:12.846487', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'Charger', 'Master.Authentication.User', '主管日常管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('3', '2019-05-23 19:49:12.909966', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'Miner', 'Master.Authentication.User', '矿工日常管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('4', '2019-05-23 19:49:12.958796', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'CaseSource', 'Master.Case.CaseSource', '判例库', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('5', '2019-05-23 19:49:13.017392', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'NewMiner', 'Master.Authentication.NewMiner', '审核新矿工', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('6', '2019-05-23 19:49:13.080871', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'User', 'Master.Authentication.User', '用户管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('7', '2019-05-23 19:49:13.157046', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'NewSource', 'Master.Module.ModuleData', '判例新增', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('8', '2019-05-23 19:49:13.204900', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'SourceImport', 'Master.Module.ModuleData', '导入判例', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('9', '2019-05-23 19:49:13.260566', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'CaseList', 'Master.Module.ModuleData', '成品案例管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('10', '2019-05-23 19:49:13.288887', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'Types', 'Master.Module.ModuleData', '分类内容管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('11', '2019-05-23 19:49:13.317209', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'Labels', 'Master.Module.ModuleData', '标签内容管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('12', '2019-05-23 19:49:13.378734', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'ChargerTeam', 'Master.Module.ModuleData', '主管团队管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('13', '2019-05-23 19:49:13.418775', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'Organization', 'Master.Module.ModuleData', '组织架构', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('14', '2019-05-23 19:49:13.445143', null, null, null, '\0', null, null, '{\"PluginName\":\"admin\"}', '1', '', null, 'Role', 'Master.Module.ModuleData', '角色管理', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('15', '2019-05-23 19:49:13.487137', null, null, null, '\0', null, null, '{\"PluginName\":\"setting\"}', '1', '', null, 'BaseSetting', 'Master.Module.ModuleData', '基本设置', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('16', '2019-05-23 19:49:13.519365', null, null, null, '\0', null, null, '{\"PluginName\":\"setting\"}', '1', '', null, 'ModuleSetting', 'Master.Module.ModuleData', '模块设置', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('17', '2019-05-23 19:49:13.573078', null, null, null, '\0', null, null, '{\"PluginName\":\"setting\"}', '1', '', null, 'MenuSetting', 'Master.Module.ModuleData', '菜单设置', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('18', '2019-05-23 19:49:13.605306', null, null, null, '\0', null, null, '{\"PluginName\":\"setting\"}', '1', '', null, 'DictionarySetting', 'Master.Module.ModuleData', '字典设置', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('19', '2019-05-26 11:13:09.067291', null, null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'CaseInitial', 'Master.Case.CaseInitial', '成品案例', '10', '[10,15,50,100]', 'Id', '1');
INSERT INTO `moduleinfo` VALUES ('20', '2019-05-28 22:36:36.041187', '2', null, null, '\0', null, null, '{\"PluginName\":\"core\"}', '1', '', null, 'Assistant', 'Master.Authentication.User', '助理日常管理', '10', '[10,15,50,100]', 'Id', '1');

-- ----------------------------
-- Table structure for newminer
-- ----------------------------
DROP TABLE IF EXISTS `newminer`;
CREATE TABLE `newminer` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `OpenId` longtext,
  `Avata` longtext,
  `NickName` longtext,
  `Name` longtext,
  `WorkLocation` longtext,
  `Email` longtext,
  `CreationTime` datetime(6) NOT NULL,
  `Remarks` longtext,
  `Verified` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_NewMiner_CreatorUserId` (`CreatorUserId`),
  KEY `IX_NewMiner_DeleterUserId` (`DeleterUserId`),
  KEY `IX_NewMiner_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_NewMiner_TenantId` (`TenantId`),
  CONSTRAINT `FK_NewMiner_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_NewMiner_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_NewMiner_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_NewMiner_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of newminer
-- ----------------------------

-- ----------------------------
-- Table structure for notice
-- ----------------------------
DROP TABLE IF EXISTS `notice`;
CREATE TABLE `notice` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `NoticeTitle` longtext,
  `NoticeContent` longtext,
  `IsActive` bit(1) NOT NULL,
  `NoticeType` longtext,
  `ToTenantId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Notice_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Notice_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Notice_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Notice_TenantId` (`TenantId`),
  CONSTRAINT `FK_Notice_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_Notice_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Notice_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Notice_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of notice
-- ----------------------------

-- ----------------------------
-- Table structure for organization
-- ----------------------------
DROP TABLE IF EXISTS `organization`;
CREATE TABLE `organization` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) NOT NULL,
  `ParentId` int(11) DEFAULT NULL,
  `Code` longtext,
  `DisplayName` longtext NOT NULL,
  `BriefCode` longtext,
  `Sort` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Organization_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Organization_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Organization_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Organization_ParentId` (`ParentId`),
  KEY `IX_Organization_TenantId` (`TenantId`),
  CONSTRAINT `FK_Organization_Organization_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `organization` (`id`),
  CONSTRAINT `FK_Organization_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Organization_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Organization_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Organization_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of organization
-- ----------------------------
INSERT INTO `organization` VALUES ('1', '2019-05-23 19:49:12.322053', null, null, null, '\0', null, null, null, null, null, '1', null, '00001', '系统管理', null, '0');

-- ----------------------------
-- Table structure for permissions
-- ----------------------------
DROP TABLE IF EXISTS `permissions`;
CREATE TABLE `permissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsGranted` bit(1) NOT NULL,
  `Discriminator` longtext NOT NULL,
  `RoleId` int(11) DEFAULT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Permissions_TenantId_Name` (`TenantId`,`Name`),
  KEY `IX_Permissions_RoleId` (`RoleId`),
  KEY `IX_Permissions_UserId` (`UserId`),
  CONSTRAINT `FK_Permissions_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `role` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Permissions_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_Permissions_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of permissions
-- ----------------------------
INSERT INTO `permissions` VALUES ('1', '2019-05-23 19:49:12.081810', null, '1', 'Menu.Admin.Tenancy.NewSource', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('2', '2019-05-23 19:49:12.111108', null, '1', 'Menu.Setting.Tenancy.DictionarySetting', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('3', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Setting.Tenancy.MenuSetting', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('4', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Setting.Tenancy.ModuleSetting', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('5', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Setting.Tenancy.BaseSetting', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('6', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.User', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('7', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Role', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('8', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Organization', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('9', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Assistant', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('10', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.ChargerTeam', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('11', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Charger', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('12', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Miner', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('13', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.NewMiner', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('14', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Labels', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('15', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.Types', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('16', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.CaseList', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('17', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.CaseSource', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('18', '2019-05-23 19:49:12.110131', null, '1', 'Menu.Admin.Tenancy.SourceImport', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('19', '2019-05-23 19:49:12.111108', null, '1', 'Menu.Setting.Tenancy.SystemLog', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('20', '2019-05-23 19:49:12.111108', null, '1', 'Menu.Setting.Tenancy.LoginLog', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('21', '2019-05-28 22:33:10.627049', '2', '1', 'Module.Assistant.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('22', '2019-05-28 22:33:10.671973', '2', '1', 'Module.CaseInitial.Button.Back', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('23', '2019-05-28 22:33:10.670996', '2', '1', 'Module.User.Button.Account', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('24', '2019-05-28 22:33:10.669043', '2', '1', 'Module.User.Button.Permission', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('25', '2019-05-28 22:33:10.668066', '2', '1', 'Module.User.Button.Edit', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('26', '2019-05-28 22:33:10.666113', '2', '1', 'Module.User.Button.Add', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('27', '2019-05-28 22:33:10.665137', '2', '1', 'Module.User.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('28', '2019-05-28 22:33:10.664160', '2', '1', 'Module.NewMiner.Button.Verify', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('29', '2019-05-28 22:33:10.662207', '2', '1', 'Module.NewMiner.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('30', '2019-05-28 22:33:10.661230', '2', '1', 'Module.CaseSource.Button.UnFreeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('31', '2019-05-28 22:33:10.659277', '2', '1', 'Module.CaseSource.Button.Freeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('32', '2019-05-28 22:33:10.658300', '2', '1', 'Module.CaseSource.Button.Edit', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('33', '2019-05-28 22:33:10.673926', '2', '1', 'Module.CaseInitial.Button.Down', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('34', '2019-05-28 22:33:10.656347', '2', '1', 'Module.CaseSource.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('35', '2019-05-28 22:33:10.653417', '2', '1', 'Module.Miner.Button.Freeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('36', '2019-05-28 22:33:10.652441', '2', '1', 'Module.Miner.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('37', '2019-05-28 22:33:10.650488', '2', '1', 'Module.Charger.Button.UnFreeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('38', '2019-05-28 22:33:10.649511', '2', '1', 'Module.Charger.Button.Freeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('39', '2019-05-28 22:33:10.647558', '2', '1', 'Module.Charger.Button.Edit', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('40', '2019-05-28 22:33:10.646581', '2', '1', 'Module.Charger.Button.Add', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('41', '2019-05-28 22:33:10.645605', '2', '1', 'Module.Charger.Button.Delete', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('42', '2019-05-28 22:33:10.643651', '2', '1', 'Module.Assistant.Button.UnFreeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('43', '2019-05-28 22:33:10.642675', '2', '1', 'Module.Assistant.Button.Freeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('44', '2019-05-28 22:33:10.640722', '2', '1', 'Module.Assistant.Button.Edit', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('45', '2019-05-28 22:33:10.639745', '2', '1', 'Module.Assistant.Button.Add', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('46', '2019-05-28 22:33:10.655371', '2', '1', 'Module.Miner.Button.UnFreeze', '', 'RolePermissionSetting', '2', null);
INSERT INTO `permissions` VALUES ('47', '2019-05-28 22:33:10.675879', '2', '1', 'Module.CaseInitial.Button.Recommand', '', 'RolePermissionSetting', '2', null);

-- ----------------------------
-- Table structure for resource
-- ----------------------------
DROP TABLE IF EXISTS `resource`;
CREATE TABLE `resource` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `ResourceName` longtext,
  `ResourceType` longtext,
  `ResourceContent` longtext,
  `IsActive` bit(1) NOT NULL,
  `Status` longtext,
  `Sort` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Resource_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Resource_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Resource_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Resource_TenantId` (`TenantId`),
  CONSTRAINT `FK_Resource_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_Resource_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Resource_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Resource_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of resource
-- ----------------------------

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `Name` longtext,
  `DisplayName` longtext,
  `IsStatic` bit(1) NOT NULL,
  `IsDefault` bit(1) NOT NULL,
  `Remarks` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_Role_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Role_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Role_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Role_TenantId` (`TenantId`),
  CONSTRAINT `FK_Role_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_Role_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Role_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Role_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES ('1', '2019-05-23 19:49:11.000713', null, null, null, '\0', null, null, null, 'Admin', 'Admin', '', '\0', null);
INSERT INTO `role` VALUES ('2', '2019-05-23 19:49:11.957781', null, null, null, '\0', null, null, '1', 'Admin', '系统管理员', '', '\0', null);
INSERT INTO `role` VALUES ('3', '2019-05-23 19:49:11.985126', null, null, null, '\0', null, null, '1', 'Assistant', '系统助理', '', '\0', null);
INSERT INTO `role` VALUES ('4', '2019-05-23 19:49:12.005635', null, null, null, '\0', null, null, '1', 'Manager', '业务总管', '', '\0', null);
INSERT INTO `role` VALUES ('5', '2019-05-23 19:49:12.018331', null, null, null, '\0', null, null, '1', 'Charger', '业务主管', '', '\0', null);
INSERT INTO `role` VALUES ('6', '2019-05-23 19:49:12.030050', null, null, null, '\0', null, null, '1', 'Miner', '矿工', '', '\0', null);

-- ----------------------------
-- Table structure for settings
-- ----------------------------
DROP TABLE IF EXISTS `settings`;
CREATE TABLE `settings` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_Settings_TenantId_Name` (`TenantId`,`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of settings
-- ----------------------------

-- ----------------------------
-- Table structure for template
-- ----------------------------
DROP TABLE IF EXISTS `template`;
CREATE TABLE `template` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Remarks` longtext,
  `Property` json DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `TemplateName` longtext,
  `TemplateType` longtext,
  `TemplateContent` longtext,
  `IsActive` bit(1) NOT NULL,
  `Status` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_Template_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Template_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Template_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_Template_TenantId` (`TenantId`),
  CONSTRAINT `FK_Template_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_Template_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Template_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Template_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of template
-- ----------------------------

-- ----------------------------
-- Table structure for tenant
-- ----------------------------
DROP TABLE IF EXISTS `tenant`;
CREATE TABLE `tenant` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `EditionId` int(11) DEFAULT NULL,
  `TenancyName` longtext,
  `Name` longtext,
  `ConnectionString` longtext,
  `IsActive` bit(1) NOT NULL,
  `Property` json DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Tenant_CreatorUserId` (`CreatorUserId`),
  KEY `IX_Tenant_DeleterUserId` (`DeleterUserId`),
  KEY `IX_Tenant_EditionId` (`EditionId`),
  KEY `IX_Tenant_LastModifierUserId` (`LastModifierUserId`),
  CONSTRAINT `FK_Tenant_Edition_EditionId` FOREIGN KEY (`EditionId`) REFERENCES `edition` (`id`),
  CONSTRAINT `FK_Tenant_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Tenant_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_Tenant_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of tenant
-- ----------------------------
INSERT INTO `tenant` VALUES ('1', '2019-05-23 19:49:11.837660', null, null, null, '\0', null, null, null, 'Default', 'Default', null, '', null);

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint(20) DEFAULT NULL,
  `IsDeleted` bit(1) NOT NULL,
  `DeleterUserId` bigint(20) DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `UserName` longtext,
  `Name` longtext,
  `Sex` varchar(2) DEFAULT NULL,
  `Password` longtext,
  `PhoneNumber` longtext,
  `WorkLocation` longtext,
  `BirthDay` datetime(6) DEFAULT NULL,
  `Email` longtext,
  `OrganizationId` int(11) DEFAULT NULL,
  `IsActive` bit(1) NOT NULL,
  `ToBeVerified` bit(1) NOT NULL,
  `LockoutEndDate` datetime(6) DEFAULT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `LastLoginTime` datetime(6) DEFAULT NULL,
  `ExtensionData` longtext,
  `Property` json DEFAULT NULL,
  `Status` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_User_CreatorUserId` (`CreatorUserId`),
  KEY `IX_User_DeleterUserId` (`DeleterUserId`),
  KEY `IX_User_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_User_OrganizationId` (`OrganizationId`),
  KEY `IX_User_TenantId` (`TenantId`),
  CONSTRAINT `FK_User_Organization_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `organization` (`id`),
  CONSTRAINT `FK_User_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_User_User_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_User_User_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `user` (`id`),
  CONSTRAINT `FK_User_User_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('1', '2019-05-23 19:49:11.648199', null, null, null, '\0', null, null, null, 'admin', 'admin', null, 'p8ysTBBU+SBjdQxFwWO8wQ==', null, null, null, null, null, '', '\0', null, '0', null, null, null, null);
INSERT INTO `user` VALUES ('2', '2019-05-23 19:49:12.192165', null, '2019-05-30 12:37:58.485114', null, '\0', null, null, '1', 'admin', 'admin', null, 'p8ysTBBU+SBjdQxFwWO8wQ==', null, null, null, null, null, '', '\0', null, '0', '2019-05-30 12:37:58.082755', '{\"currentToken\":\"wNYmO41/48SHNstaLVXxHCCre29BZQl1NhC6NM3R3rzpXtPQxVzH6jEzA/QhXFN5tu6Fk7pO53uppm1mVXMZgxbyRVz26dnepi/FyB6axBY+6gq1GL+uRQgoiFUCjRN2p8w6LevViwKlHyWZZJZO1DGVSjAi1m2U+og9pkHw9/SNWYTNNhoJdxzCsgThjt5dfd0IMavtYqIoRmvJzFdbCdb1NrRM8Knd2AEVhcw2Xc5I6bY+cHibSIDwBSqjBOpZINrTc1saF0VvmRG8G2fo+NAetlVlNWwiCghOvic1CMpbuSMhMnAPtO0xK5wEfJ/EP/dSOl3o5lBaAN0FK1O4SkmXX4fFe0skoi/fusSeZbKgF4bAKaQGqNoZo9+9ikBlXmy+hSAtPHyn2pqu6a+Hsf3usp9twjE1tww46xwp/tbTDiWsRfKEDaK8DQqJdB97/ucuQJQvc1hlzxuWraqGrgPSDC+vSD5SGMwALr/fBtoHkKYN9GjwdBE6oRpc+kswC/m5pB4eTYDFKXThfZiqnON8MhN6saRGtfRzuh/S/+T0HoCncMU+wciOIFlXZjv6hv5MiScJaWTOF73tMFevT581Jkw70qdC08KugUEBdrSesO5cUSRwDGfR+FpdTgMF1gvnVs6sJsUdRKtEGecpidOdGd9CU8ZObSyR9I1OFh2jdye1VtDXwnMmip8QALZyuewj6aVPdgCTbfVOfN8z7q54APCua3EQjzORohuJLanfXQ/zWLwNSp+9L03e8BjX\"}', null, null);

-- ----------------------------
-- Table structure for userlogin
-- ----------------------------
DROP TABLE IF EXISTS `userlogin`;
CREATE TABLE `userlogin` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TenantId` int(11) DEFAULT NULL,
  `UserId` bigint(20) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserLogin_UserId` (`UserId`),
  CONSTRAINT `FK_UserLogin_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of userlogin
-- ----------------------------

-- ----------------------------
-- Table structure for userloginattempt
-- ----------------------------
DROP TABLE IF EXISTS `userloginattempt`;
CREATE TABLE `userloginattempt` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TenantId` int(11) DEFAULT NULL,
  `TenancyName` varchar(255) DEFAULT NULL,
  `UserId` bigint(20) DEFAULT NULL,
  `UserNameOrPhoneNumber` varchar(255) DEFAULT NULL,
  `ClientIpAddress` longtext,
  `ClientName` longtext,
  `BrowserInfo` longtext,
  `Result` tinyint(3) unsigned NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserLoginAttempt_UserId_TenantId` (`UserId`,`TenantId`),
  KEY `IX_UserLoginAttempt_TenancyName_UserNameOrPhoneNumber_Result` (`TenancyName`,`UserNameOrPhoneNumber`,`Result`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of userloginattempt
-- ----------------------------
INSERT INTO `userloginattempt` VALUES ('1', '1', 'Default', '2', 'admin', '101.66.141.77', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', '1', '2019-05-23 19:54:42.329842');
INSERT INTO `userloginattempt` VALUES ('2', '1', 'Default', '2', 'admin', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:66.0) Gecko/20100101 Firefox/66.0', '1', '2019-05-23 20:03:20.169818');
INSERT INTO `userloginattempt` VALUES ('3', '1', 'Default', '2', 'admin', '112.17.235.189', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-23 20:05:24.574942');
INSERT INTO `userloginattempt` VALUES ('4', '1', 'Default', '2', 'admin', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', '3', '2019-05-26 11:16:11.870209');
INSERT INTO `userloginattempt` VALUES ('5', '1', 'Default', '2', 'admin', '39.181.180.145', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', '1', '2019-05-26 11:16:18.481791');
INSERT INTO `userloginattempt` VALUES ('6', '1', 'Default', '2', 'admin', '123.158.16.27', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', '1', '2019-05-26 22:48:04.718206');
INSERT INTO `userloginattempt` VALUES ('7', '1', 'Default', '2', 'admin', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-27 06:51:40.694165');
INSERT INTO `userloginattempt` VALUES ('8', '1', 'Default', '2', 'admin', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-27 17:19:29.731302');
INSERT INTO `userloginattempt` VALUES ('9', '1', 'Default', '2', 'admin', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-27 19:36:18.139266');
INSERT INTO `userloginattempt` VALUES ('10', '1', 'Default', '2', 'admin', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 SE 2.X MetaSr 1.0', '1', '2019-05-27 21:57:00.448881');
INSERT INTO `userloginattempt` VALUES ('11', '1', 'Default', '2', 'admin', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-27 22:11:48.017073');
INSERT INTO `userloginattempt` VALUES ('12', '1', 'Default', '2', 'admin', '116.235.253.157', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-27 22:16:22.800267');
INSERT INTO `userloginattempt` VALUES ('13', '1', 'Default', '2', 'admin', '223.104.255.108', null, 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-28 16:34:11.468893');
INSERT INTO `userloginattempt` VALUES ('14', '1', 'Default', '2', 'admin', '123.153.121.205', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-28 22:29:46.466866');
INSERT INTO `userloginattempt` VALUES ('15', '1', 'Default', '2', 'admin', '183.246.170.52', null, 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36', '1', '2019-05-30 12:37:58.226315');

-- ----------------------------
-- Table structure for userrole
-- ----------------------------
DROP TABLE IF EXISTS `userrole`;
CREATE TABLE `userrole` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint(20) DEFAULT NULL,
  `TenantId` int(11) DEFAULT NULL,
  `UserId` bigint(20) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserRole_UserId` (`UserId`),
  KEY `IX_UserRole_TenantId_RoleId` (`TenantId`,`RoleId`),
  KEY `IX_UserRole_TenantId_UserId` (`TenantId`,`UserId`),
  CONSTRAINT `FK_UserRole_Tenant_TenantId` FOREIGN KEY (`TenantId`) REFERENCES `tenant` (`id`),
  CONSTRAINT `FK_UserRole_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of userrole
-- ----------------------------
INSERT INTO `userrole` VALUES ('1', '2019-05-23 19:49:11.754649', null, null, '1', '1');
INSERT INTO `userrole` VALUES ('2', '2019-05-23 19:49:12.230253', null, '1', '2', '2');

-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
INSERT INTO `__efmigrationshistory` VALUES ('20190522120459_Init', '2.2.2-servicing-10034');
INSERT INTO `__efmigrationshistory` VALUES ('20190524124907_CaseInitial', '2.2.2-servicing-10034');
INSERT INTO `__efmigrationshistory` VALUES ('20190525154045_Update', '2.2.2-servicing-10034');
