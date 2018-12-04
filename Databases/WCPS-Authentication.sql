/*
SQLyog Community v13.1.1 (64 bit)
MySQL - 10.1.29-MariaDB : Database - wcps-authentication
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`wcps-authentication` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;

USE `wcps-authentication`;

/*Table structure for table `server_statistics` */

DROP TABLE IF EXISTS `server_statistics`;

CREATE TABLE `server_statistics` (
  `ID` smallint(6) NOT NULL,
  `name` varchar(20) DEFAULT NULL,
  `type` tinyint(4) DEFAULT NULL,
  `users_online` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `server_statistics` */

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(20) CHARACTER SET latin1 DEFAULT NULL,
  `displayname` char(16) CHARACTER SET latin1 DEFAULT NULL,
  `password` char(64) CHARACTER SET latin1 DEFAULT NULL,
  `salt` char(10) CHARACTER SET latin1 DEFAULT NULL,
  `rights` tinyint(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

/*Data for the table `users` */

insert  into `users`(`ID`,`username`,`displayname`,`password`,`salt`,`rights`) values 
(1,'test','DarkRaptor','B94B7E2F9546BA176839730DD30FDF659338A07F8F313E12F7E117E0752172DA','asd',1),
(2,'test2','darkito','B94B7E2F9546BA176839730DD30FDF659338A07F8F313E12F7E117E0752172DA','asd',0);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
