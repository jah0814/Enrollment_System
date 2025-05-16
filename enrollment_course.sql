-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: enrollment
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `course`
--

DROP TABLE IF EXISTS `course`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `course` (
  `course_id` int NOT NULL AUTO_INCREMENT,
  `subject_code` varchar(20) DEFAULT NULL,
  `subject_name` varchar(100) DEFAULT NULL,
  `units` int DEFAULT NULL,
  `program_id` int DEFAULT NULL,
  `semester_id` int DEFAULT NULL,
  PRIMARY KEY (`course_id`),
  KEY `program_id` (`program_id`),
  KEY `semester_id` (`semester_id`),
  CONSTRAINT `course_ibfk_1` FOREIGN KEY (`program_id`) REFERENCES `program` (`program_id`),
  CONSTRAINT `course_ibfk_2` FOREIGN KEY (`semester_id`) REFERENCES `semester` (`semester_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `course`
--

LOCK TABLES `course` WRITE;
/*!40000 ALTER TABLE `course` DISABLE KEYS */;
INSERT INTO `course` VALUES (1,'BA101','Principles of Management',3,1,1),(2,'BA102','Business Law and Ethics',3,1,1),(3,'BA103','Financial Accounting',3,1,2),(4,'IT101','Programming Languages',3,2,2),(5,'IT102','Database Management Systems',3,2,3),(6,'IT103','Operating Systems',3,2,1),(7,'NU101','Anatomy and Physiology',4,3,2),(8,'NU102','Pharmacology',3,3,3),(9,'NU103','Medical-Surgical Nursing',4,3,1),(10,'CE101','Structural Analysis',4,4,3),(11,'CE102','Fluid Mechanics',4,4,2),(12,'CE103','Geotechnical Engineering',4,4,1),(13,'FA101','Art History',3,5,1),(14,'FA102','Drawing and Painting',3,5,2),(15,'FA103','Sculpture',3,5,3),(16,'NU201','Advanced Medical-Surgical Nursing',4,6,1),(17,'NU202','Geriatric Nursing',3,6,2),(18,'NU203','Oncology Nursing',3,6,3),(19,'CE201','Structural Steel Design',4,7,1),(20,'CE202','Advanced Reinforced Concrete',4,7,2),(21,'CE203','Earthquake Engineering',3,7,3),(22,'CE301','Highway Engineering',4,8,1),(23,'CE302','Traffic Flow Theory',3,8,2),(24,'CE303','Transportation Planning',3,8,3),(25,'FA201','Graphic Design',3,9,1),(26,'FA202','Visual Narrative',3,9,2),(27,'FA203','Digital Illustration',3,9,3),(28,'FA301','Product Design',3,10,1),(29,'FA302','Design Ergonomics',3,10,2),(30,'FA303','CAD for Industrial Design',3,10,3);
/*!40000 ALTER TABLE `course` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-27 18:38:48
