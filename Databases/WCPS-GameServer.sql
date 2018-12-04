/*
SQLyog Community v13.1.1 (64 bit)
MySQL - 10.1.29-MariaDB : Database - wcps-server
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`wcps-server` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `wcps-server`;

/*Table structure for table `item_weapons` */

DROP TABLE IF EXISTS `item_weapons`;

CREATE TABLE `item_weapons` (
  `id` smallint(5) unsigned NOT NULL,
  `code` varchar(4) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '0',
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `item_weapons` */

insert  into `item_weapons`(`id`,`code`,`active`,`name`) values 
(0,'DA01',0,'DA_M7'),
(1,'DA02',1,'DA_KNUCKLE'),
(2,'DA03',0,'DA_STILETTO'),
(3,'DA04',0,'DA_SWORD'),
(7,'DB01',1,'DB_COLT'),
(8,'DB02',1,'DB_DESERT_EG'),
(9,'DB03',1,'DB_MP5K'),
(10,'DB04',1,'DB_MAGNUM'),
(11,'DB05',1,'DB_GLOCK'),
(12,'DB06',1,'DB_BERETTA_D'),
(13,'DB07',0,'DB_THROWING_KNIFE'),
(16,'DC01',1,'DC_AK47'),
(17,'DC02',1,'DC_K2'),
(18,'DC03',1,'DC_M4A1'),
(19,'DC04',1,'DC_FAMAS'),
(20,'DC05',1,'DC_L85A1'),
(21,'DC06',0,'DC_XM8'),
(22,'DC07',0,'DC_TYPE89'),
(23,'DC08',0,'DC_SIG550'),
(24,'DC09',1,'DC_TAR_21'),
(28,'DD01',1,'DD_G36C'),
(29,'DD02',1,'DD_G36C_D'),
(31,'DE01',1,'DE_G36'),
(32,'DE02',0,'DE_G36_D'),
(33,'DF01',1,'DF_MP5'),
(34,'DF02',1,'DF_P90'),
(35,'DF03',1,'DF_UZI'),
(36,'DF04',1,'DF_TMP9'),
(37,'DF05',1,'DF_K1'),
(38,'DF06',1,'DF_MP7A1'),
(39,'DF07',1,'DF_SCORPION_D'),
(40,'DF08',1,'DF_Spectre_M4'),
(44,'DG01',1,'DG_PSG_1'),
(45,'DG02',1,'DG_BARRETT_M82'),
(46,'DG03',1,'DG_AUG'),
(47,'DG04',1,'DG_SSG'),
(48,'DG05',1,'DG_M24'),
(49,'DG06',0,'DG_DRAGUNOV_SVD'),
(50,'DG07',1,'DG_AI_AW'),
(51,'DG08',1,'DG_AW50F'),
(56,'DH01',1,'DH_M60'),
(57,'DH02',1,'DH_M249'),
(58,'DI01',1,'DI_WINCHESTER_1300'),
(60,'DJ01',1,'DJ_PZF_3'),
(61,'DJ02',0,'DJ_M136AT_4'),
(62,'DJ03',1,'DJ_RPG_7'),
(63,'DJ04',0,'DJ_JAVELIN'),
(64,'DK01',1,'DK_STINGER'),
(65,'DK02',0,'DK_SG'),
(66,'DL01',1,'DL_TMA_1A'),
(67,'DL02',0,'DL_HA_SUPPLY'),
(68,'DM01',1,'DM_K400_GRENADE'),
(69,'DN01',1,'DN_K400_GRENADE_ASSULT'),
(70,'DO01',1,'DO_SMOKE_G'),
(71,'DO02',1,'DO_FLASH_BANG_1'),
(72,'DO03',0,'DO_FLASH_BANG_2'),
(73,'DP01',1,'DP_CLAYMORE'),
(74,'DP02',1,'DP_CLAYMORE_SWITCH'),
(75,'DP03',0,'DP_PDA'),
(76,'DP04',0,'DP_SWITCH_C4'),
(77,'DQ01',1,'DQ_MEDIC_KIT_1'),
(78,'DQ02',0,'DQ_MEDIC_KIT_2'),
(79,'DQ03',0,'DQ_MEDIC_KIT_3'),
(80,'DR01',1,'DR_SPANNER'),
(81,'DR02',1,'DR_PIPE_WRENCH'),
(82,'DS01',1,'DS_ADRENALINE'),
(83,'DS02',1,'DS_PARACHUTE'),
(84,'DS03',0,'DS_STAMINA'),
(86,'DX02',0,'DS_DETECTOR'),
(87,'DX01',1,'DS_TELESCOPE'),
(88,'DS05',1,'DS_FLASH_MINE'),
(89,'DH01',0,'DT_MG3'),
(90,'DH03',1,'DT_M134'),
(91,'DT03',1,'DT_MK1S'),
(92,'DT04',0,'DT_HK69'),
(93,'DU01',1,'DU_AMMO_BOX'),
(94,'DU02',1,'DU_M14'),
(95,'DU03',0,'DU_TEARGAS'),
(96,'DV01',1,'DV_MEDIC_BOX'),
(97,'DW01',0,'DW_K203'),
(98,'DW02',0,'DW_TELESCOPE'),
(99,'DW03',0,'DW_SILENCER'),
(100,'DW04',0,'DU_NIPPERS'),
(101,'DZ01',0,'DZ_HA_SUPPLY'),
(102,'D001',0,'D0_DRUM_PIECE'),
(103,'D101',1,'D1_SLOT_CHG_6TH');

/*Table structure for table `user_stats` */

DROP TABLE IF EXISTS `user_stats`;

CREATE TABLE `user_stats` (
  `ID` bigint(20) unsigned NOT NULL,
  `kills` bigint(20) unsigned NOT NULL DEFAULT '0',
  `deaths` bigint(20) unsigned NOT NULL DEFAULT '0',
  `headshots` bigint(20) unsigned NOT NULL DEFAULT '0',
  `bombs_planted` bigint(20) unsigned NOT NULL DEFAULT '0',
  `bombs_defused` bigint(20) unsigned NOT NULL DEFAULT '0',
  `rounds_played` bigint(20) unsigned NOT NULL DEFAULT '0',
  `flags_taken` bigint(20) unsigned NOT NULL DEFAULT '0',
  `victories` bigint(20) unsigned NOT NULL DEFAULT '0',
  `defeats` bigint(20) unsigned NOT NULL DEFAULT '0',
  `vehicles_destroyed` bigint(20) unsigned NOT NULL DEFAULT '0',
  KEY `user_stats_ibfk_1` (`ID`),
  CONSTRAINT `user_stats_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `users` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `user_stats` */

insert  into `user_stats`(`ID`,`kills`,`deaths`,`headshots`,`bombs_planted`,`bombs_defused`,`rounds_played`,`flags_taken`,`victories`,`defeats`,`vehicles_destroyed`) values 
(1,10,1,0,0,0,0,0,0,0,0);

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `ID` bigint(20) unsigned NOT NULL,
  `xp` bigint(20) unsigned NOT NULL DEFAULT '0',
  `money` int(10) unsigned NOT NULL DEFAULT '30000',
  `premium` tinyint(3) unsigned DEFAULT '0',
  `premium_expiredate` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `users` */

insert  into `users`(`ID`,`xp`,`money`,`premium`,`premium_expiredate`) values 
(1,0,30000,0,0);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
