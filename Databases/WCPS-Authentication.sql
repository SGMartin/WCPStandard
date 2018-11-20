/*
SQLyog Community v12.5.0 (64 bit)
MySQL - 10.1.29-MariaDB : Database - wcps-authentication
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`wcps-authentication` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `wcps-authentication`;

/*Table structure for table `server-statistics` */

DROP TABLE IF EXISTS `server-statistics`;

CREATE TABLE `server-statistics` (
  `ID` smallint(6) NOT NULL,
  `name` varchar(20) DEFAULT NULL,
  `type` tinyint(4) DEFAULT NULL,
  `users_online` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `server-statistics` */

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(20) DEFAULT NULL,
  `displayname` char(16) DEFAULT NULL,
  `password` char(64) DEFAULT NULL,
  `salt` char(10) DEFAULT NULL,
  `rights` tinyint(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Data for the table `users` */

insert  into `users`(`ID`,`username`,`displayname`,`password`,`salt`,`rights`) values 
(1,'test','WCPS','B94B7E2F9546BA176839730DD30FDF659338A07F8F313E12F7E117E0752172DA','asd',1);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
